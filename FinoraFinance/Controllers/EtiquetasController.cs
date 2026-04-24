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
    public class EtiquetasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public EtiquetasController(AppDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Etiquetas/Index
        public async Task<IActionResult> Index(string search = "", int page = 1, int pageSize = 10)
        {
            var userId = _userManager.GetUserId(User);

            var connectionString = _context.Database.GetDbConnection().ConnectionString;
            var repo = new EtiquetaRepository(connectionString);
            var (items, total) = await repo.FiltrarPaginadoAsync(search, page, pageSize, userId);


            ViewBag.Search = search ?? "";
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRegistros = total;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / pageSize);


            System.Diagnostics.Debug.WriteLine($"=== ETIQUETAS DEBUG ===");
            System.Diagnostics.Debug.WriteLine($"Search: {search}");
            System.Diagnostics.Debug.WriteLine($"Page: {page}");
            System.Diagnostics.Debug.WriteLine($"Total: {total}");
            System.Diagnostics.Debug.WriteLine($"TotalPaginas: {ViewBag.TotalPaginas}");

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> CrearAjax([FromBody] CrearEtiquetaRequest request)
        {
            var userId = _userManager.GetUserId(User);
            var etiqueta = new Etiqueta
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                UserId = userId
            };
            _context.Etiquetas.Add(etiqueta);
            await _context.SaveChangesAsync();
            return Json(new { success = true, id = etiqueta.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var etiqueta = await _context.Etiquetas
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (etiqueta == null)
            {
                TempData["Error"] = "Etiqueta no encontrada";
                return RedirectToAction(nameof(Index));
            }

            var tieneTransacciones = await _context.Transacciones
                .AnyAsync(t => t.EtiquetaId == id && t.UserId == userId);

            if (tieneTransacciones)
            {
                TempData["Error"] = "No puedes eliminar esta etiqueta porque tiene transacciones asociadas.";
                return RedirectToAction(nameof(Index));
            }

            _context.Etiquetas.Remove(etiqueta);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Etiqueta eliminada exitosamente";
            return RedirectToAction(nameof(Index));
        }
    }

    public class CrearEtiquetaRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}