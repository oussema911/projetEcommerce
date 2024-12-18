namespace projetEcommerce.Models
{
    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; } // Optionnel pour confirmer le mot de passe
    }

}
