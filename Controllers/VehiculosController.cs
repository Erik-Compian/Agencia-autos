using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; // Necesario para el .Include()
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para las listas desplegables (ComboBox)
using AgenciaAutosMVC.Models;

namespace AgenciaAutosMVC.Controllers
{
    [Authorize] // Lo protegemos para que solo entre el administrador
    public class VehiculosController : Controller
    {
        private readonly AgenciaAutosContext _context;

        public VehiculosController(AgenciaAutosContext context)
        {
            _context = context;
        }

        // GET: Vehiculos
        public IActionResult Index()
        {
            // POO Pura: Traemos la lista de vehículos, pero le pedimos a Entity Framework
            // que también "incluya" los objetos relacionados (Dueño y Modelo)
            var listaVehiculos = _context.Vehiculos
                .Include(v => v.IdClienteNavigation) // Trae los datos del Cliente
                .Include(v => v.IdModeloNavigation)  // Trae los datos del Modelo
                .ThenInclude(m => m.IdMarcaNavigation) // Y del modelo, trae la Marca
                .ToList();

            return View(listaVehiculos);
        }

        // --- NUEVOS MÉTODOS PARA EL CREATE ---

        // GET: Dibuja el formulario y carga las listas desplegables
        [HttpGet]
        public IActionResult Create()
        {
            // Extraemos los clientes de MySQL y armamos una lista. 
            // "IdCliente" es el número que se guarda en BD, "Nombre" es el texto que ve el usuario.
            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre");

            // Hacemos lo mismo con los modelos
            ViewBag.Modelos = new SelectList(_context.Modelos, "IdModelo", "Nombre");

            return View();
        }

        // POST: Recibe los datos desde el HTML, los guarda en MySQL y regresa a la tabla
        [HttpPost]
        public IActionResult Create(Vehiculo nuevoVehiculo)
        {
            // 1. Le decimos al modelo que ignore los objetos complejos porque esos no viajan por internet, solo viajan los IDs.
            ModelState.Remove("IdClienteNavigation");
            ModelState.Remove("IdModeloNavigation");
            ModelState.Remove("Servicios"); // Ignoramos la lista de servicios futuros

            // 2. Ahora sí, validamos
            if (ModelState.IsValid)
            {
                _context.Vehiculos.Add(nuevoVehiculo);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // Si hay un error real, volvemos a cargar las listas y mostramos el formulario
            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre");
            ViewBag.Modelos = new SelectList(_context.Modelos, "IdModelo", "Nombre");
            return View(nuevoVehiculo);
        }

        // --- MÉTODOS PARA EDITAR (UPDATE) ---

        // GET: Muestra el formulario con los datos actuales del vehículo
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var vehiculo = _context.Vehiculos.Find(id);
            if (vehiculo == null) return NotFound();

            // Cargamos las listas y pre-seleccionamos la opción actual del vehículo
            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre", vehiculo.IdCliente);
            ViewBag.Modelos = new SelectList(_context.Modelos, "IdModelo", "Nombre", vehiculo.IdModelo);

            return View(vehiculo);
        }

        // POST: Recibe los cambios y los actualiza en MySQL
        [HttpPost]
        public IActionResult Edit(Vehiculo vehiculoActualizado)
        {
            ModelState.Remove("IdClienteNavigation");
            ModelState.Remove("IdModeloNavigation");
            ModelState.Remove("Servicios");

            if (ModelState.IsValid)
            {
                // POO Pura: Entity Framework sabe que debe hacer un UPDATE y no un INSERT por el ID
                _context.Vehiculos.Update(vehiculoActualizado);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre", vehiculoActualizado.IdCliente);
            ViewBag.Modelos = new SelectList(_context.Modelos, "IdModelo", "Nombre", vehiculoActualizado.IdModelo);
            return View(vehiculoActualizado);
        }


        // --- MÉTODOS PARA ELIMINAR (DELETE) ---

        // GET: Muestra la pantalla de confirmación (Pide el objeto completo para mostrar nombres)
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var vehiculo = _context.Vehiculos
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdModeloNavigation)
                .ThenInclude(m => m.IdMarcaNavigation)
                .FirstOrDefault(v => v.IdVehiculo == id);

            if (vehiculo == null) return NotFound();

            return View(vehiculo);
        }

        // POST: Ejecuta la eliminación real en MySQL
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var vehiculo = _context.Vehiculos.Find(id);
            if (vehiculo != null)
            {
                _context.Vehiculos.Remove(vehiculo);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }

}