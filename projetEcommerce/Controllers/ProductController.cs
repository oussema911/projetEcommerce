using Microsoft.AspNetCore.Mvc;
using projetEcommerce.Data;
using projetEcommerce.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace projetEcommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action Index pour afficher la liste des produits avec pagination et filtres
        public IActionResult Index(int? page, string search, decimal? minPrice, decimal? maxPrice)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            int pageSize = 6;
            int pageNumber = page ?? 1;

            return View(products.OrderBy(p => p.Name).ToPagedList(pageNumber, pageSize));
        }

        // Action Details pour afficher les détails d'un produit
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
