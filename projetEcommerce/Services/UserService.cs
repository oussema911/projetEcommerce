using projetEcommerce.Models;
using projetEcommerce.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace projetEcommerce.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Méthode pour obtenir l'ID de l'utilisateur connecté
        public int? GetUserId()
        {
            // Récupérer l'utilisateur connecté depuis le HttpContext
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.Session == null)
                return null;

            // Récupérer l'ID de l'utilisateur depuis la session
            var userId = httpContext.Session.GetInt32("UserId");
            return userId;
        }

        // Méthode pour définir l'ID de l'utilisateur dans la session lors de la connexion
        public void SetUserIdInSession(int userId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            httpContext?.Session.SetInt32("UserId", userId);
        }

        // Méthode pour effacer l'ID de l'utilisateur lors de la déconnexion
        public void ClearUserSession()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            httpContext?.Session.Remove("UserId");
        }

        // Méthode pour obtenir un utilisateur par ID
        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        // Méthode pour enregistrer un utilisateur
        public void RegisterUser(string username, string password, string email)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username || u.Email == email);
            if (existingUser != null)
            {
                throw new Exception("Utilisateur déjà existant");
            }
            var user = new User(username, password, email);
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // Méthode pour authentifier un utilisateur
        public User AuthenticateUser(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Définir l'ID de l'utilisateur dans la session après authentification
                SetUserIdInSession(user.Id);
            }

            return user;
        }
    }
}