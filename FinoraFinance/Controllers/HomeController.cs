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
                var usuario = await _userManager.FindByIdAsync(userId);  
                var hoy = DateTime.Now;
                var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
                var finMes = inicioMes.AddMonths(1).AddDays(-1);

                var cuentas = await _context.Cuentas
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                var transacciones = await _context.Transacciones
                    .Include(t => t.Etiqueta)
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.Fecha)
                    .ToListAsync();

                var transaccionesMes = transacciones
                    .Where(t => t.Fecha >= inicioMes && t.Fecha <= finMes)
                    .Count();

                var ultimasTransacciones = transacciones.Take(5).ToList();

                var etiquetas = await _context.Etiquetas
                    .Where(e => e.UserId == userId)
                    .OrderBy(e => e.Nombre)
                    .ToListAsync();

                var ultimasEtiquetas = etiquetas.Take(10).ToList();

                ViewBag.FotoPerfil = usuario?.FotoPerfil;  
                ViewBag.TotalCuentas = cuentas.Count;
                ViewBag.SaldoTotal = cuentas.Sum(c => c.SaldoInicial);
                ViewBag.UltimasCuentas = cuentas.OrderByDescending(c => c.Id).Take(5).ToList();
                ViewBag.TransaccionesMes = transaccionesMes;
                ViewBag.UltimasTransacciones = ultimasTransacciones;
                ViewBag.UltimasEtiquetas = ultimasEtiquetas;
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