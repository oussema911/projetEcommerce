using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetEcommerce.Data;
using projetEcommerce.Models;
using projetEcommerce.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace projetEcommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderService _orderService;

        // Injecter le contexte de base de données et le service OrderService
        public OrderController(ApplicationDbContext context, OrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        // Afficher la confirmation de la commande (GET)
        [HttpGet]
        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order); // Passer l'objet 'Order' au modèle de la vue
        }

        // Ajouter des lignes à la commande (POST)
        [HttpPost]
        public async Task<IActionResult> AddOrderItems(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Ajouter des articles dans la table OrderItem
            foreach (var product in order.OrderItems.Select(oi => oi.Product).Distinct())
            {
                var orderItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = 1, // Vous pouvez ajuster la quantité ici si nécessaire
                    TotalPrice = product.Price // Calculer le total en fonction du produit
                };

                // Ajouter la ligne à la table OrderItem
               // _context.OrderItem.Add(orderItem);
            }

            // Sauvegarder les changements dans la base de données
            await _context.SaveChangesAsync();

            // Optionnellement, rediriger vers une page de confirmation ou d'autres détails
            return RedirectToAction("OrderDetails", new { orderId = order.Id });
        }

        // Afficher les détails de la commande après la confirmation
        public IActionResult OrderDetails(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order); // Passer l'objet 'Order' au modèle de la vue
        }
    }
}
