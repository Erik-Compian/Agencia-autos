using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using AgenciaAutosMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgenciaAutosMVC.Controllers
{
    [Authorize] // 2. ESTE ES EL CANDADO. Bloquea todo el controlador.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
