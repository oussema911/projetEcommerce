using Microsoft.EntityFrameworkCore;
using projetEcommerce.Data;
using projetEcommerce.Services; // Espaces de noms pour les services n�cessaires
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Configurer la connexion � la base de donn�es avec SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection_toDb")));  // Remplacer par la cha�ne de connexion appropri�e

// Ajouter les services n�cessaires pour les contr�leurs et les vues
builder.Services.AddControllersWithViews();

// Enregistrer les services pour l'injection de d�pendances
builder.Services.AddHttpContextAccessor();  // Activer l'acc�s � HttpContext pour la gestion de la session
builder.Services.AddScoped<CartService>();  // Service pour la gestion du panier
builder.Services.AddScoped<UserService>();  // Service pour la gestion des utilisateurs
builder.Services.AddScoped<OrderService>();  // Service pour la gestion des commandes
builder.Services.AddScoped<ProductService>();  // Service pour g�rer les produits

builder.Services.AddLogging();
// Ajouter la configuration pour l'authentification par cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  // Rediriger vers la page de connexion si l'utilisateur n'est pas authentifi�
        options.LogoutPath = "/Account/Logout";  // Rediriger vers la page de d�connexion
        options.AccessDeniedPath = "/Account/AccessDenied";  // Page d'acc�s refus�
    });

// Configurer la session avec un d�lai d'expiration (ex: 30 minutes)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Dur�e de vie de la session
    options.Cookie.HttpOnly = true;  // S�curiser les cookies
    options.Cookie.IsEssential = true;  // Marquer le cookie comme essentiel
});

var app = builder.Build();

// Configurer le pipeline de requ�tes HTTP
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
app.UseAuthentication();  // Middleware pour g�rer l'authentification
app.UseAuthorization();  // Middleware pour g�rer l'autorisation

app.UseRouting();

// Configurer la route par d�faut pour les contr�leurs
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
