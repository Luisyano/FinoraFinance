using FinoraFinance.Data;
using FinoraFinance.Models;
using FinoraFinance.Repositories;
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
        public async Task<IActionResult> Lista(string search = "", string orden = "fecha_desc", int page = 1, int pageSize = 10)
        {
            var userId = _userManager.GetUserId(User);
            var connectionString = _context.Database.GetDbConnection().ConnectionString;
            var repo = new TransaccionRepository(connectionString);

            var (items, total) = await repo.FiltrarPaginadoAsync(search, orden, page, pageSize, userId);

            ViewBag.Search = search ?? "";
            ViewBag.Orden = orden;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRegistros = total;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / pageSize);

            var transacciones = items.Select(dto => new Transaccion
            {
                Id = dto.Id,
                Monto = dto.Monto,
                Fecha = dto.Fecha,
                Tipo = dto.Tipo,
                Nota = dto.Nota,
                CuentaId = dto.CuentaId,
                EtiquetaId = dto.EtiquetaId,
                UserId = dto.UserId,
                Cuenta = new Cuenta { Nombre = dto.CuentaNombre, Moneda = dto.Moneda },
                Etiqueta = new Etiqueta { Nombre = dto.EtiquetaNombre }
            }).ToList();

            return View("Index", transacciones); 
        }
    }
}