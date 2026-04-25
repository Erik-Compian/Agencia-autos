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
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] 
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
        [HttpGet]
        public IActionResult Index(string buscar)
        {
            var consulta = _context.Servicios
                .Include(s => s.IdVehiculoNavigation)
                    .ThenInclude(v => v.IdModeloNavigation)
                        .ThenInclude(m => m.IdMarcaNavigation)
                .Include(s => s.IdVehiculoNavigation)
                    .ThenInclude(v => v.IdClienteNavigation)
                .Include(s => s.IdTipoServNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                string b = buscar.ToLower().Trim();

                // Intentamos ver si lo que escribió el usuario es un número (un Folio)
                bool esNumero = int.TryParse(b, out int folioBusqueda);

                if (esNumero)
                {
                    // Si es un número, buscamos ÚNICAMENTE el Folio exacto
                    // Esto evita que se confunda con los números de las placas
                    consulta = consulta.Where(s => s.Folio == folioBusqueda);
                }
                else
                {
                    // Si tiene letras, buscamos en Nombre, Apellido o Placa
                    consulta = consulta.Where(s =>
                        s.IdVehiculoNavigation.IdClienteNavigation.Nombre.ToLower().Contains(b) ||
                        s.IdVehiculoNavigation.IdClienteNavigation.Apellido.ToLower().Contains(b) ||
                        s.IdVehiculoNavigation.Placa.ToLower().Contains(b)
                    );
                }

                ViewBag.BusquedaActual = buscar;
            }

            var listaServicios = consulta.OrderByDescending(s => s.Folio).ToList();
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
            // POO Pura: Buscamos el objeto original, pero ahora INCLUIMOS el vehículo para saber su kilometraje
            var servicioBD = _context.Servicios
                .Include(s => s.IdVehiculoNavigation)
                .FirstOrDefault(s => s.Folio == Folio);

            if (servicioBD != null)
            {
                // Solo modificamos lo que el administrador cambió en la pantalla
                servicioBD.Estatus = Estatus;
                servicioBD.Descripcion = Descripcion;

                // Lógica de negocio: Si ya lo finalizó, le ponemos la fecha de salida automática
                if (Estatus == "Finalizado" && servicioBD.FechaSalida == null)
                {
                    servicioBD.FechaSalida = DateOnly.FromDateTime(DateTime.Now);

                    // --- INICIO LÓGICA AUTOMÁTICA PRÓXIMO SERVICIO (Punto 9) ---
                    bool yaExiste = _context.ProximoServicios.Any(p => p.Folio == Folio);
                    if (!yaExiste)
                    {
                        var recordatorio = new ProximoServicio
                        {
                            Folio = Folio,
                            // Le sumamos 6 meses a la fecha de hoy
                            FechaProg = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                            // Le sumamos 10,000 km al kilometraje actual del auto
                            KmProximo = servicioBD.IdVehiculoNavigation.KmActual + 10000,
                            Notas = "Sistema: Recordatorio automático (6 meses o 10,000 km)."
                        };
                        _context.ProximoServicios.Add(recordatorio);
                    }
                    // --- FIN LÓGICA AUTOMÁTICA PRÓXIMO SERVICIO ---
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
        [HttpGet]
        public IActionResult Comprobante(int? id)
        {
            if (id == null) return NotFound();

            var servicio = _context.Servicios
                .Include(s => s.IdVehiculoNavigation)
                    .ThenInclude(v => v.IdModeloNavigation)
                        .ThenInclude(m => m.IdMarcaNavigation)
                .Include(s => s.IdVehiculoNavigation)
                    .ThenInclude(v => v.IdClienteNavigation)
                .Include(s => s.IdTipoServNavigation)
               
                .Include(s => s.ServicioRefaccions)
                    .ThenInclude(sr => sr.IdRefaccionNavigation)
                // ------------------------------------------
                .FirstOrDefault(s => s.Folio == id);

            if (servicio == null) return NotFound();

            return View(servicio);
        }
        // GET: Listado de próximos servicios (Punto 9 de la Rúbrica)
        public IActionResult Proximos(string buscar)
        {
            // 1. Preparamos la consulta base (Aún no va a la base de datos)
            var consulta = _context.ProximoServicios
                .Include(p => p.FolioNavigation)
                    .ThenInclude(s => s.IdVehiculoNavigation)
                    .ThenInclude(v => v.IdClienteNavigation)
                .AsQueryable();

            // 2. Si el usuario escribió algo en el buscador, aplicamos los filtros
            if (!string.IsNullOrEmpty(buscar))
            {
                consulta = consulta.Where(p =>
                    p.Folio.ToString().Contains(buscar) ||
                    p.FolioNavigation.IdVehiculoNavigation.IdClienteNavigation.Nombre.Contains(buscar) ||
                    p.FolioNavigation.IdVehiculoNavigation.IdClienteNavigation.Apellido.Contains(buscar));
            }

            // 3. Ejecutamos la consulta ordenando por la fecha más próxima
            var proximos = consulta.OrderBy(p => p.FechaProg).ToList();

            return View(proximos);
        }
        // GET: Pantalla para gestionar refacciones de un servicio
        public IActionResult Refacciones(int id)
        {
            // Buscamos el servicio y cargamos sus refacciones actuales
            var servicio = _context.Servicios
                .Include(s => s.ServicioRefaccions)
                    .ThenInclude(sr => sr.IdRefaccionNavigation)
                .FirstOrDefault(s => s.Folio == id);

            if (servicio == null) return NotFound();

            // Pasamos la lista de todas las refacciones disponibles para el dropdown
            ViewBag.CatalogoRefacciones = _context.Refaccions.ToList();

            return View(servicio);
        }

        // 1. Agregar Refacción y Restar Inventario
        [HttpPost]
        public IActionResult AgregarRefaccion(int folio, int idRefaccion, int cantidad)
        {
            var refaccion = _context.Refaccions.Find(idRefaccion);

            if (refaccion != null)
            {
                // Validación de coherencia: No podemos vender lo que no tenemos
                if (refaccion.Stock < cantidad)
                {
                    TempData["Error"] = $"Solo quedan {refaccion.Stock} unidades de {refaccion.Nombre}.";
                    return RedirectToAction("Refacciones", new { id = folio });
                }

                // Creamos el registro en la tabla intermedia
                var detalle = new ServicioRefaccion
                {
                    Folio = folio,
                    IdRefaccion = idRefaccion,
                    Cantidad = cantidad,
                    PrecioAplicado = refaccion.Precio // Usamos 'Precio' de tu modelo Refaccion
                };

                // Restamos del inventario (Stock es int, no necesita cast)
                refaccion.Stock -= cantidad;

                _context.ServicioRefaccions.Add(detalle);
                _context.SaveChanges();
            }

            return RedirectToAction("Refacciones", new { id = folio });
        }
        // 2. Eliminar Refacción y Devolver al Inventario
        [HttpPost]
        public IActionResult EliminarRefaccion(int folio, int idRefaccion)
        {
            // Buscamos por la llave compuesta: Folio + IdRefaccion
            var detalle = _context.ServicioRefaccions
                .Include(sr => sr.IdRefaccionNavigation)
                .FirstOrDefault(sr => sr.Folio == folio && sr.IdRefaccion == idRefaccion);

            if (detalle != null)
            {
                // Devolvemos las piezas al estante (Coherencia total)
                detalle.IdRefaccionNavigation.Stock += detalle.Cantidad;

                _context.ServicioRefaccions.Remove(detalle);
                _context.SaveChanges();
            }

            return RedirectToAction("Refacciones", new { id = folio });
        }
        // POST: Eliminar un servicio
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            var servicioBD = _context.Servicios.Find(id);

            if (servicioBD != null)
            {
                _context.Servicios.Remove(servicioBD);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}