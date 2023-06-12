using Catalog.Data;
using Catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;


namespace Catalog.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext _context;
        private Cart _cart;

        public CartController(ApplicationDbContext context, Cart cart)
        {
            _context = context;
            _cart = cart;

        }
        public IActionResult Index()
        {
            ViewBag.categories = _context.Categories.ToList();

            //_cart = HttpContext.Session.Get<Cart>("cart");
            return View(_cart.Items.Values.ToList());
        }


        [Authorize]
        public IActionResult Add(int id, string returnUrl)
        {
            ViewBag.categories = _context.Categories.ToList();

            //_cart = HttpContext.Session.Get<Cart>("cart");
            var item = _context.Credits.Find(id);
            if (item != null)
            {
                _cart.AddToCart(item);
                //HttpContext.Session.Set<Cart>("cart", _cart);
            }
            return Redirect(returnUrl);
        }

        public IActionResult Delete(int id)
        {
            ViewBag.categories = _context.Categories.ToList();

            //_cart = HttpContext.Session.Get<Cart>("cart");
            _cart.RemoveFromCart(id);
            //HttpContext.Session.Set<Cart>("cart", _cart);
            return RedirectToAction("Index");
        }
    }
}