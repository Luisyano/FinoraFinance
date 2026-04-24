using FinoraFinance.Models;
using FinoraFinance.Models.Vista;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinoraFinance.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(
            
            UserManager<Usuario> userManager,
            
            SignInManager<Usuario> signInManager,
             IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }


        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Registrarse() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrarse(Registrarse model)
        {
            
            if (!ModelState.IsValid) return View(model);
            var user = new Usuario
            {
                UserName = model.Email,  
                Email = model.Email,
                Telefono = model.Telefono,
                FullName = model.FullName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                TempData["Msg"] = "¡Registro Exitoso!";
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        [HttpGet] 
        public IActionResult IniciarSesion() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IniciarSesion(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                    return View(model);
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user.UserName,
                        model.Password,
                        model.RememberMe,
                        false
                        );
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                            return View(model);
                        }
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/EditarPerfil
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditarPerfil()
        {
            var userId = _userManager.GetUserId(User);
            var usuario = await _userManager.FindByIdAsync(userId);

            if (usuario == null)
            {
                return NotFound();
            }

            var model = new EditarPerfil
            {
                Email = usuario.Email ?? string.Empty,
                FullName = usuario.FullName ?? string.Empty,
                Telefono = usuario.Telefono ?? string.Empty,
                FotoActual = usuario.FotoPerfil
            };

            return View(model);
        }

        // POST: Account/EditarPerfil
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPerfil(EditarPerfil model, IFormFile? foto)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            var usuario = await _userManager.FindByIdAsync(userId);

            if (usuario == null)
            {
                return NotFound();
            }


            usuario.FullName = model.FullName;
            usuario.Telefono = model.Telefono;

            if (foto != null && foto.Length > 0)
            {
                
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "perfiles");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }


                var fileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                {
                    var oldPath = Path.Combine(uploadsFolder, usuario.FotoPerfil);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                usuario.FotoPerfil = fileName;
            }

            var result = await _userManager.UpdateAsync(usuario);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Perfil actualizado correctamente";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}
