using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgenciaAutosMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace AgenciaAutosMVC.Controllers
{
    [Authorize]
    public class CatalogosController : Controller
    {
        private readonly AgenciaAutosContext _context;

        public CatalogosController(AgenciaAutosContext context)
        {
            _context = context;
        }

        // Pantalla Principal de Catálogos
        public async Task<IActionResult> Index()
        {
            ViewBag.Marcas = await _context.Marcas.ToListAsync();
            // Incluimos la navegación de marca para mostrar el nombre en la tabla
            ViewBag.Modelos = await _context.Modelos.Include(m => m.IdMarcaNavigation).ToListAsync();

            // Lista desplegable de marcas para el formulario de modelos
            ViewBag.ListaMarcas = new SelectList(await _context.Marcas.ToListAsync(), "IdMarca", "Nombre");

            return View();
        }

        // Guardar Nueva Marca
        [HttpPost]
        public async Task<IActionResult> NuevaMarca(string nombre)
        {
            if (!string.IsNullOrEmpty(nombre))
            {
                _context.Marcas.Add(new Marca { Nombre = nombre });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Guardar Nuevo Modelo incluyendo el Año
        [HttpPost]
        public async Task<IActionResult> NuevoModelo(string nombre, int idMarca, int anio)
        {
            if (!string.IsNullOrEmpty(nombre) && idMarca > 0)
            {
                _context.Modelos.Add(new Modelo
                {
                    Nombre = nombre,
                    IdMarca = idMarca,
                    // Agregamos (short) antes de la variable para solucionar el error
                    Anio = (short)anio
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        // --- MÉTODO PARA ELIMINAR MARCA ---
        [HttpPost]
        public async Task<IActionResult> EliminarMarca(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca != null)
            {
                try
                {
                    _context.Marcas.Remove(marca);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    // Opcional: Aquí podrías mandar un mensaje si no se puede borrar porque tiene modelos asociados
                    TempData["Error"] = "No se puede eliminar una marca que tiene modelos registrados.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // --- MÉTODO PARA ELIMINAR MODELO ---
        [HttpPost]
        public async Task<IActionResult> EliminarModelo(int id)
        {
            var modelo = await _context.Modelos.FindAsync(id);
            if (modelo != null)
            {
                _context.Modelos.Remove(modelo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // --- SECCIÓN DE REFACCIONES ---

        [HttpGet]
        public IActionResult Refacciones(string buscar)
        {
            var consulta = _context.Refaccions.AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                string b = buscar.ToLower().Trim();

                
                bool esNumero = int.TryParse(b, out int idBuscado);

                if (esNumero)
                {
                 
                    consulta = consulta.Where(r => r.IdRefaccion == idBuscado || r.Nombre.ToLower().Contains(b));
                }
                else
                {
                    
                    consulta = consulta.Where(r => r.Nombre.ToLower().Contains(b));
                }

                ViewBag.BusquedaActual = buscar;
            }

            return View(consulta.OrderBy(r => r.Stock).ToList());
        }

        // Para editar precio o surtir stock
        [HttpGet]
        public IActionResult EditarRefaccion(int id)
        {
            var refaccion = _context.Refaccions.Find(id);
            if (refaccion == null) return NotFound();
            return View(refaccion);
        }

        [HttpPost]
        public IActionResult EditarRefaccion(Refaccion refaccion)
        {
            // 1. Buscamos la refacción original directamente en la base de datos
            var refaccionEnBD = _context.Refaccions.Find(refaccion.IdRefaccion);

            if (refaccionEnBD != null)
            {
                // 2. Solo actualizamos los campos que nos importan
                refaccionEnBD.Precio = refaccion.Precio;
                refaccionEnBD.Stock = refaccion.Stock;

                // 3. Guardamos los cambios a la fuerza
                _context.SaveChanges();

                // 4. Redirigimos a la lista
                return RedirectToAction("Refacciones");
            }

            // Si por alguna razón no la encuentra, regresa la vista
            return View(refaccion);
        }
        // GET: Catalogos/CrearRefaccion
        [HttpGet]
        public IActionResult CrearRefaccion()
        {
            return View();
        }

        // POST: Catalogos/CrearRefaccion
        [HttpPost]
        public IActionResult CrearRefaccion(Refaccion refaccion)
        {
            if (ModelState.IsValid)
            {
                _context.Refaccions.Add(refaccion);
                _context.SaveChanges();
                return RedirectToAction("Refacciones");
            }
            return View(refaccion);
        }
    }
}