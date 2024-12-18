using Microsoft.AspNetCore.Mvc;
using projetEcommerce.Models;
using projetEcommerce.Services;

namespace projetEcommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;

        public CartController(CartService cartService, ProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        // Ajouter un produit au panier
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            // Récupérer le produit par son ID
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();  // Si le produit n'existe pas, renvoyer une erreur 404
            }

            // Ajouter le produit au panier
            _cartService.AddToCart(product);

            // Rediriger vers la vue du panier
            //return RedirectToAction("ViewCart");
            return RedirectToAction("index", "Product");  // Redirige vers la vue du panier s'il est vide

        }

        // Afficher le panier
        public IActionResult ViewCart()
        {
            var cartItems = _cartService.GetCartItems();

            // Calculer le total
            decimal total = 0;
            foreach (var item in cartItems)
            {
                total += item.Price * item.Quantity;
            }

            // Passer le total à la vue
            ViewData["Total"] = total.ToString("0.00");

            return View("Cart", cartItems);
        }

        // Mettre à jour la quantité d'un produit dans le panier
        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            _cartService.UpdateCart(productId, quantity);
            return RedirectToAction("ViewCart");
        }

        // Supprimer un produit du panier
        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            return RedirectToAction("ViewCart");
        }
        public IActionResult Checkout()
        {
            // Vous pouvez récupérer les informations de commande ou de facture ici.
            var cartItems = _cartService.GetCartItems();

            // Vérifiez si le panier est vide et redirigez si nécessaire
            if (!cartItems.Any())
            {
                return RedirectToAction("ViewCart", "Cart");  // Redirige vers la vue du panier s'il est vide
            }

            // Calcul du total de la commande
            decimal total = 0;
            foreach (var item in cartItems)
            {
                total += item.Price * item.Quantity;
            }

            // Passez les informations nécessaires à la vue (ex: total, panier)
            ViewData["Total"] = total.ToString("0.00");

            return View(cartItems); // Assurez-vous d'avoir une vue Checkout.cshtml
        }
        [HttpPost]
        public IActionResult OrderConfirmation()
        {
            // Simply return the view for order confirmation
            return View();
        }

    }
}
