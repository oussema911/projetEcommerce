using projetEcommerce.Data;
using projetEcommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace projetEcommerce.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        // Injection du contexte de base de données
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Méthode pour récupérer un produit par son ID
        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId); // Récupère le produit par ID
        }

        // Méthode pour récupérer tous les produits
        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync(); // Récupère tous les produits
        }
    }
}
