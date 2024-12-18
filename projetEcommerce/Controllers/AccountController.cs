using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using projetEcommerce.Models;
using projetEcommerce.Services;
using System.Security.Claims;

namespace projetEcommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly OrderService _orderService;

        public AccountController(UserService userService, OrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        // Page d'inscription
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Enregistrement d'un utilisateur
        [HttpPost]
        public IActionResult Register(string username, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(string.Empty, "Tous les champs sont obligatoires.");
                return View();
            }

            try
            {
                _userService.RegisterUser(username, password, email);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        // Page de connexion
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Connexion de l'utilisateur
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _userService.AuthenticateUser(username, password);
            if (user != null)
            {
                // Créer une liste de "claims" pour l'utilisateur
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                // Créer une identité avec les claims
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Se connecter avec la méthode SignInAsync
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return RedirectToAction("Profile");
            }

            ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect.");
            return View();
        }

        // Déconnexion
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Page de profil utilisateur
        public IActionResult Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserProfileViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                //Orders = _orderService.GetOrdersByUserId(userId)
            };

            return View(viewModel);
        }
    }
}
