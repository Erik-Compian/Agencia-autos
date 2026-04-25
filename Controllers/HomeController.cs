using AgenciaAutosMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AgenciaAutosMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AgenciaAutosContext _context;

        public HomeController(ILogger<HomeController> logger, AgenciaAutosContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(DateTime? fechaFiltro)
        {
            // 1. Filtro base de Servicios para la Gráfica
            var query = _context.Servicios.AsQueryable();

            if (fechaFiltro.HasValue)
            {
                DateOnly fechaComparar = DateOnly.FromDateTime(fechaFiltro.Value);
                query = query.Where(s => s.FechaIngreso == fechaComparar);
                ViewBag.FiltroActual = fechaFiltro.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.FiltroActual = "Total Histórico";
            }

            // 2. Datos para la Gráfica (basados en el filtro)
            ViewBag.EnEspera = query.Count(s => s.Estatus == "En espera");
            ViewBag.EnProceso = query.Count(s => s.Estatus == "En proceso");
            ViewBag.Finalizados = query.Count(s => s.Estatus == "Finalizado");

            // 3. Cálculos de Dinero (Solo de servicios Finalizados)
            decimal manoObraBase = 1200;

            var serviciosConDinero = _context.Servicios
                .Include(s => s.ServicioRefaccions)
                    .ThenInclude(sr => sr.IdRefaccionNavigation)
                .Where(s => s.Estatus == "Finalizado")
                .AsQueryable();

            if (fechaFiltro.HasValue)
            {
                DateOnly f = DateOnly.FromDateTime(fechaFiltro.Value);
                serviciosConDinero = serviciosConDinero.Where(s => s.FechaIngreso == f);
            }

            decimal totalVentas = 0;
            // Usamos ToList() aquí para evitar errores de tipo al procesar los datos
            var listaFinalizados = serviciosConDinero.ToList();

            foreach (var s in listaFinalizados)
            {
                totalVentas += manoObraBase;
                totalVentas += s.ServicioRefaccions.Sum(sr => sr.Cantidad * (sr.IdRefaccionNavigation?.Precio ?? 0));
            }
            ViewBag.TotalVentas = totalVentas;

            // 4. Datos Globales
            ViewBag.TotalVehiculos = _context.Vehiculos.Count();
            ViewBag.TotalClientes = _context.Clientes.Count();

            // 5. Alerta de Inventario (Menos de 6 piezas)
            var refaccionesBajas = _context.Refaccions.Where(r => r.Stock <= 5).ToList();
            ViewBag.StockCriticoCount = refaccionesBajas.Count;
            ViewBag.ListaRefaccionesBajas = refaccionesBajas;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}