using Microsoft.EntityFrameworkCore;
using projetEcommerce.Data;
using projetEcommerce.Services; // Espaces de noms pour les services nécessaires
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Configurer la connexion à la base de données avec SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection_toDb")));  // Remplacer par la chaîne de connexion appropriée

// Ajouter les services nécessaires pour les contrôleurs et les vues
builder.Services.AddControllersWithViews();

// Enregistrer les services pour l'injection de dépendances
builder.Services.AddHttpContextAccessor();  // Activer l'accès à HttpContext pour la gestion de la session
builder.Services.AddScoped<CartService>();  // Service pour la gestion du panier
builder.Services.AddScoped<UserService>();  // Service pour la gestion des utilisateurs
builder.Services.AddScoped<OrderService>();  // Service pour la gestion des commandes
builder.Services.AddScoped<ProductService>();  // Service pour gérer les produits

builder.Services.AddLogging();
// Ajouter la configuration pour l'authentification par cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  // Rediriger vers la page de connexion si l'utilisateur n'est pas authentifié
        options.LogoutPath = "/Account/Logout";  // Rediriger vers la page de déconnexion
        options.AccessDeniedPath = "/Account/AccessDenied";  // Page d'accès refusé
    });

// Configurer la session avec un délai d'expiration (ex: 30 minutes)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Durée de vie de la session
    options.Cookie.HttpOnly = true;  // Sécuriser les cookies
    options.Cookie.IsEssential = true;  // Marquer le cookie comme essentiel
});

var app = builder.Build();

// Configurer le pipeline de requêtes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Activer l'utilisation de la session
app.UseSession();

// Ajouter les middlewares d'authentification et d'autorisation
app.UseAuthentication();  // Middleware pour gérer l'authentification
app.UseAuthorization();  // Middleware pour gérer l'autorisation

app.UseRouting();

// Configurer la route par défaut pour les contrôleurs
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
