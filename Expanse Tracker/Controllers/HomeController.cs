using Expanse_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Expanse_Tracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Construtor que recebe um ILogger para permitir a integração com logs.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Ação para a página inicial (Index).
        public IActionResult Index()
        {
            return View();
        }

        // Ação para a página de privacidade (Privacy).
        public IActionResult Privacy()
        {
            return View();
        }

        // Ação para lidar com erros.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Retorna uma view de erro, passando um objeto ErrorViewModel com informações sobre o erro.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
