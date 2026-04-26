using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AgenciaAutosMVC.Models;

namespace AgenciaAutosMVC.Controllers
{
    [Authorize] // Protegemos la pantalla para que solo entren los logueados
    public class ClientesController : Controller
    {
        private readonly AgenciaAutosContext _context;

        public ClientesController(AgenciaAutosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string buscar)
        {
            // Iniciamos la consulta
            var consulta = _context.Clientes.AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                string b = buscar.ToLower().Trim();

                // Filtramos por Nombre, Apellido, Teléfono o Email
                consulta = consulta.Where(c =>
                    c.Nombre.ToLower().Contains(b) ||
                    c.Apellido.ToLower().Contains(b) ||
                    c.Telefono.Contains(b) ||
                    c.Email.ToLower().Contains(b)
                );

                ViewBag.BusquedaActual = buscar;
            }

            return View(consulta.ToList());
        }

        // GET: Mostrar el formulario para un cliente nuevo
        public IActionResult Nuevo()
        {
            return View();
        }

        // POST: Guardar el cliente en la base de datos
        [HttpPost]
        public IActionResult Nuevo(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }
        // GET: Muestra el formulario con los datos actuales del cliente
        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null) return NotFound();

            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // POST: Guarda los cambios en la base de datos
        [HttpPost]
        public IActionResult Editar(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Update(cliente);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // POST: Elimina el cliente de la base de datos
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}