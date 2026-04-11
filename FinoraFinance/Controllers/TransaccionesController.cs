using FinoraFinance.Data;
using FinoraFinance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinoraFinance.Controllers
{
    [Authorize]
    public class TransaccionesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public TransaccionesController(AppDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);


            ViewBag.Cuentas = await _context.Cuentas
                .Where(c => c.UserId == userId)
                .ToListAsync();

            ViewBag.Etiquetas = await _context.Etiquetas
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaccion transaccion)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(transaccion.Tipo))
            {
                transaccion.Tipo = "Ingreso";
            }

            if (!ModelState.IsValid)
            {
                var errores = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                TempData["ErrorDetalle"] = $"Error de validación: {errores}";

                ViewBag.Cuentas = await _context.Cuentas.Where(c => c.UserId == userId).ToListAsync();
                ViewBag.Etiquetas = await _context.Etiquetas.Where(e => e.UserId == userId).ToListAsync();
                return View(transaccion);
            }
            transaccion.UserId = userId;

            var cuenta = await _context.Cuentas.FindAsync(transaccion.CuentaId);
            if (cuenta == null)
            {
                TempData["Error"] = "Cuenta no encontrada";
                return RedirectToAction("Create");
            }

            if (transaccion.Tipo == "Ingreso")
            {
                cuenta.SaldoInicial += transaccion.Monto;
            }
            else
            {
                cuenta.SaldoInicial -= transaccion.Monto;
            }

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registro añadido exitosamente";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var transacciones = await _context.Transacciones
                .Include(t => t.Cuenta)
                .Include(t => t.Etiqueta)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();

            return View(transacciones);
        }
    }
}