using System;
using System.Collections.Generic;

namespace projetEcommerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
       

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal TotalAmount => CalculateTotalAmount();

        private decimal CalculateTotalAmount()
        {
            return OrderItems != null
                ? OrderItems.Sum(item => item.TotalPrice)
                : 0;
        }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}