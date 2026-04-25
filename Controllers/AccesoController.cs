using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using AgenciaAutosMVC.Models;

namespace AgenciaA.Controllers
{
    public class AccesoController : Controller
    {
        // 1. Inyectamos la base de datos
        private readonly AgenciaAutosContext _context;

        public AccesoController(AgenciaAutosContext context)
        {
            _context = context;
        }

        // 2. GET: Muestra la pantalla del Login
        [HttpGet]
        public IActionResult Index()
        {
            // ELIMINAMOS la línea que redireccionaba al Home automáticamente.
            // Ahora, sin importar qué, siempre mostrará la vista del Login.
            return View();
        }

        // 3. POST: Recibe los datos del formulario HTML
        [HttpPost]
        public async Task<IActionResult> Index(string usuario, string password)
        {
            // POO Pura: Buscamos al usuario en MySQL usando una expresión Lambda (LINQ)
            var admin = _context.Administradors.FirstOrDefault(a => a.Usuario == usuario && a.Password == password);

            if (admin != null)
            {
                // Si existe, creamos los "Claims" (Variables globales de sesión)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, admin.IdAdmin.ToString()),
                    new Claim(ClaimTypes.Name, admin.Nombre),
                    new Claim("Usuario", admin.Usuario)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Registramos la sesión en el navegador de forma encriptada
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Lo enviamos a la página principal
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Si no existe, mandamos un mensaje de error a la vista
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }
        }

        // 4. Método para Cerrar Sistema 
        public async Task<IActionResult> Salir()
        {
            //  Destruye las variables globales de sesión (Claims y Cookies de Autenticación)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //  Lo regresa al Login (Seguridad de la aplicación)
            return RedirectToAction("Index", "Acceso");

        }

    }
}
