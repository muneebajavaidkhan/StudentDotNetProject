using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    public class CartController : Controller
    {
        EcommerceContext db = new EcommerceContext();

        public IActionResult AddToCart(int productId,int qty = 1)
        {
           int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if(userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            var product = db.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null) {
                return NotFound();
            }
            //Check if Product Already in cart
            var CartItem =db.Carts.FirstOrDefault(c=>c.UserId == userId && c.ProductId == productId);
            if(CartItem != null)
            {
                //Product Exist -> Increase Quantity
                CartItem.Quantity += qty;
                CartItem.Tprice = CartItem.Quantity + product.Price;
                db.Carts.Update(CartItem);
            }
            else
            {
                //Product not in Cart -> Add new
                var cart = new Cart();
                cart.ProductId = productId;
                cart.UserId = userId;
                cart.Quantity = qty;
                cart.Tprice = product.Price * qty;

                db.Carts.Add(cart);
            }
         
            db.SaveChanges();
            return RedirectToAction("CartList");
        }
        public IActionResult CartList()
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            var items = db.Carts.Include(c => c.Product).Where(c => c.UserId == userId).ToList();
            return View(items);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
