using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // Importante: Para leer tu variable global de sesión
using AgenciaAutosMVC.Models;
using AgenciaAutosMVC.Patrones; // Importante: Para acceder a tu Factory Method

namespace AgenciaAutosMVC.Controllers
{
    [Authorize]
    public class ServiciosController : Controller
    {
        private readonly AgenciaAutosContext _context;

        public ServiciosController(AgenciaAutosContext context)
        {
            _context = context;
        }

        // 1. GET: Dibuja la pantalla de recepción
        [HttpGet]
        public IActionResult Recepcion()
        {
            // Traemos los vehículos de MySQL, uniendo el Modelo y la Placa para que sea fácil de leer en el menú
            var vehiculos = _context.Vehiculos
                .Include(v => v.IdModeloNavigation)
                .Select(v => new
                {
                    IdVehiculo = v.IdVehiculo,
                    // Mostramos "Nombre del Modelo - Placas: XYZ123"
                    Descripcion = v.IdModeloNavigation.Nombre + " - Placas: " + v.Placa
                }).ToList();

            ViewBag.Vehiculos = new SelectList(vehiculos, "IdVehiculo", "Descripcion");
            return View();
        }

        // 2. Agregamos "string quienEntrego" a los parámetros que recibe
        [HttpPost]
        public IActionResult Recepcion(int idVehiculo, string tipoMantenimiento, string quienEntrego)
        {
            int idAdminLogueado = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // 2. Le mandamos ese dato nuevo a nuestra Fábrica
            Servicio nuevoServicio = ServicioFactory.CrearServicioNuevo(tipoMantenimiento, idVehiculo, idAdminLogueado, quienEntrego);

            _context.Servicios.Add(nuevoServicio);
            _context.SaveChanges(); // ¡Aquí ya no marcará error!

            return RedirectToAction("Index", "Home");
        }

        // GET: Muestra la tabla de servicios en el taller
        [HttpGet]
        public IActionResult Index()
        {
            var listaServicios = _context.Servicios
                .Include(s => s.IdVehiculoNavigation)
                    .ThenInclude(v => v.IdModeloNavigation)
                        .ThenInclude(m => m.IdMarcaNavigation) // Traemos la marca del auto
                .Include(s => s.IdVehiculoNavigation)
                    .ThenInclude(v => v.IdClienteNavigation) // Traemos al dueño
                .Include(s => s.IdTipoServNavigation) // Traemos si es Preventivo/Correctivo
                .OrderByDescending(s => s.Folio) // Los ordenamos para ver el más nuevo primero
                .ToList();

            return View(listaServicios);
        }
        // GET: Muestra la pantalla con los datos actuales del servicio
        [HttpGet]
        public IActionResult Actualizar(int? id)
        {
            if (id == null) return NotFound();

            // Traemos el servicio y los datos del carro para mostrarlos de referencia
            var servicio = _context.Servicios
                .Include(s => s.IdVehiculoNavigation)
                    .ThenInclude(v => v.IdModeloNavigation)
                        .ThenInclude(m => m.IdMarcaNavigation)
                .FirstOrDefault(s => s.Folio == id);

            if (servicio == null) return NotFound();

            return View(servicio);
        }

        // POST: Recibe solo los campos que cambiaron y los actualiza en MySQL
        [HttpPost]
        public IActionResult Actualizar(int Folio, string Estatus, string Descripcion)
        {
            // POO Pura: Buscamos el objeto original en la base de datos
            var servicioBD = _context.Servicios.Find(Folio);

            if (servicioBD != null)
            {
                // Solo modificamos lo que el administrador cambió en la pantalla
                servicioBD.Estatus = Estatus;
                servicioBD.Descripcion = Descripcion;

                // Lógica de negocio: Si ya lo finalizó, le ponemos la fecha de salida automática
                if (Estatus == "Finalizado" && servicioBD.FechaSalida == null)
                {
                    servicioBD.FechaSalida = DateOnly.FromDateTime(DateTime.Now);
                }
                else if (Estatus != "Finalizado")
                {
                    // Si lo regresan a en proceso, quitamos la fecha de salida
                    servicioBD.FechaSalida = null;
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}