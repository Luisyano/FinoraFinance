using FinoraFinance.Data;
using FinoraFinance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FinoraFinance.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            AppDbContext context,
            UserManager<Usuario> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);

                var cuentas = await _context.Cuentas
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                ViewBag.TotalCuentas = cuentas.Count;
                ViewBag.SaldoTotal = cuentas.Sum(c => c.SaldoInicial);
                ViewBag.UltimasCuentas = cuentas.OrderByDescending(c => c.Id).Take(5).ToList();
            }
            else
            {
                ViewBag.TotalCuentas = 0;
                ViewBag.SaldoTotal = 0;
                ViewBag.UltimasCuentas = new List<Cuenta>();
            }

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