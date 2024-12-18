using Microsoft.AspNetCore.Http;
using projetEcommerce.Extensions;
using projetEcommerce.Models;
using System.Collections.Generic;
using System.Linq;

namespace projetEcommerce.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Récupérer les articles du panier depuis la session
        public List<CartItem> GetCartItems()
        {
            var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return cart;
        }

        // Ajouter un produit au panier
        public void AddToCart(Product product)
        {
            var cart = GetCartItems();

            // Vérifier si le produit est déjà dans le panier
            var cartItem = cart.FirstOrDefault(c => c.ProductId == product.Id);

            if (cartItem == null)
            {
                // Ajouter un nouveau produit au panier si non existant
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            else
            {
                // Si le produit est déjà dans le panier, augmenter la quantité
                cartItem.Quantity++;
            }

            // Sauvegarder le panier dans la session
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart);
        }

        // Mettre à jour la quantité d'un produit dans le panier
        public void UpdateCart(int productId, int quantity)
        {
            var cart = GetCartItems();

            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null && quantity > 0)
            {
                cartItem.Quantity = quantity;
            }

            // Sauvegarder les modifications dans la session
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart);
        }

        // Supprimer un produit du panier
        public void RemoveFromCart(int productId)
        {
            var cart = GetCartItems();

            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }

            // Sauvegarder les modifications dans la session
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart);
        }
    }
}
