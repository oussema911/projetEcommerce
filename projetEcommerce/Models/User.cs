namespace projetEcommerce.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;  // Mot de passe en clair
        public string Email { get; set; } = string.Empty;

        // Collection des commandes associées à cet utilisateur
        public List<Order> Orders { get; set; } = new List<Order>();

        // Constructeur sans paramètre requis par EF Core
        public User() { }

        // Constructeur personnalisé
        public User(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
