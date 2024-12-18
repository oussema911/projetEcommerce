using Microsoft.EntityFrameworkCore;
using projetEcommerce.Data;
using projetEcommerce.Models;
using System.Linq;
using System.Threading.Tasks;

namespace projetEcommerce.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderItemsAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            // Ajouter des lignes à la table OrderItem
            foreach (var product in order.OrderItems.Select(oi => oi.Product).Distinct())
            {
                var orderItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = 1, // Ajustez si nécessaire
                    TotalPrice = product.Price // Calcul du total
                };

              //  _context.OrderItems.Add(orderItem);
            }

            // Sauvegarder les modifications
            await _context.SaveChangesAsync();
        }

    }

}
