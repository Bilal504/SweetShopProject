using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetShopProject.Models;
using System.Diagnostics;

namespace SweetShopProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public SweetContext context;   

        public HomeController(ILogger<HomeController> logger, SweetContext context)
        {
            _logger = logger;
            this.context = context;

        }

        //public IActionResult Index()
        //{
        //    return View();
        //}4
        public async Task<IActionResult> Index()
        {
            var sweetContext = context.product.Include(p => p.cat).Include(p => p.city);
            return View(await sweetContext.ToListAsync());
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