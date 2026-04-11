using FinoraFinance.Data;
using FinoraFinance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var etiquetas = await _context.Etiquetas
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.Nombre)
                .ToListAsync();
            return View(etiquetas);
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
                TempData["Error"] = "No puedes eliminar esta etiqueta porque tiene transacciones asociadas. Primero elimina o modifica las transacciones.";
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