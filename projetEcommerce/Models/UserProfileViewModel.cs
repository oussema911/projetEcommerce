namespace projetEcommerce.Models
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();  // Liste des commandes de l'utilisateur
    }
}
