using FinoraFinance.Models;
using FinoraFinance.Models.Vista;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinoraFinance.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public AccountController(
            //Clase que maneja usuarios: crear, buscar, modificar, eliminar - BORRAR
            UserManager<Usuario> userManager,
            //Clase que maneja sesiones: iniciar, cerrar, verificar - BORRAR
            SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            //NOTA N3 Si algo falla, muestra el formulario otra vez con los errores.
            if (!ModelState.IsValid) return View(model);
            var user = new Usuario
            {
                UserName = model.Email,  //NOTA N4  ← Para iniciar sesión con email
                Email = model.Email,
                Telefono = model.Telefono,
                FullName = model.FullName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                TempData["SuccessMessage"] = "Usuario registrado exitosamente.";
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        [HttpGet] 
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                // NOTA 5 - _userManager.FindByEmailAsync	Busca en la base de datos un usuario con ese email
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
                            TempData["SuccessMessage"] = "Inicio de sesión exitoso."; 
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
    }
}
