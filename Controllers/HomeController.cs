using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project.Models;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EcommerceContext db;

        public HomeController(EcommerceContext context)
        {
            db = context;
        }
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var CategoeryProd = new CategoryProductVM()
            {
                Categories = db.Categories.ToList(),
                Products = db.Products.ToList()
            };


            return View(CategoeryProd);
        }

        public IActionResult ProdDetail(int id) //1102
        {
            var product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            
            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
