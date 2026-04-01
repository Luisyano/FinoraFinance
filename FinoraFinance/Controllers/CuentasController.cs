using FinoraFinance.Data;
using FinoraFinance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FinoraFinance.Controllers
{
    public class CuentasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public CuentasController(AppDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var cuentas = await _context.Cuentas
                .Where(c => c.UserId == userId)
                .ToListAsync();
            return View(cuentas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var cuenta = await _context.Cuentas
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cuenta cuenta)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Error: Usuario no autenticado";
                return RedirectToAction("IniciarSesion", "Account");
            }

            cuenta.UserId = userId;
            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();
            TempData["Msg"] = "Cuenta creada exitosamente";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var cuenta = await _context.Cuentas
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cuenta cuenta)
        {
            if (id != cuenta.Id)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var cuentaExistente = await _context.Cuentas
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (cuentaExistente == null)
            {
                return NotFound();
            }


            cuentaExistente.Nombre = cuenta.Nombre;
            cuentaExistente.Moneda = cuenta.Moneda;
            cuentaExistente.SaldoInicial = cuenta.SaldoInicial;

            await _context.SaveChangesAsync();
            TempData["Msg"] = "Cuenta actualizada exitosamente";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var cuenta = await _context.Cuentas
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var cuenta = await _context.Cuentas
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (cuenta != null)
            {
                _context.Cuentas.Remove(cuenta);
                await _context.SaveChangesAsync();
                TempData["Msg"] = "Cuenta eliminada exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CuentaExists(int id)
        {
            return _context.Cuentas.Any(e => e.Id == id);
        }
    }
}