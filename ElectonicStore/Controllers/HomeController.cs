using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ElectonicStore.Models;
using ElectonicStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ElectonicStore.Utility;
using System.Text;

namespace ElectonicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        
        [HttpGet]
        public IActionResult Index(int page)
        {
            int perPage = 3;
            if (page == 0) page = 1;
            int count = _db.Product.Count();
            int pagesCount = (int)Math.Ceiling((count + 0.0) / (perPage + 0.0));
            if (page > pagesCount) page = 1;
            ViewData["currentPage"] = page;
            ViewData["pagesCount"] = pagesCount;
            IEnumerable<Product> products = _db.Product.Include(c=>c.Category).OrderByDescending(i=>i.Id)
                .Skip((page - 1) * perPage).Take(perPage).ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            if (SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart") == null)
            {
                return Content("Ses=null");
            }
            else
            {
                List<CartModel> list = SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart");
                StringBuilder str = new StringBuilder();
                for(int i = 0; i < list.Count; i++)
                {
                    str.Append(list[i].productId);
                    str.Append("_");
                }
                return Content(str.ToString());
            }
            //return View();
        }

        [HttpGet]
        public async Task<IActionResult> Category(int id,int page)
        {
            int perPage = 3;
            if (page == 0) page = 1;
            int count = _db.Product.Where(i => i.CategoryId == id).Count();
            int pagesCount = (int)Math.Ceiling((count + 0.0) / (perPage + 0.0));
            if (page > pagesCount) page = 1;
            ViewData["currentPage"] = page;
            ViewData["pagesCount"] = pagesCount;
            ViewData["categoryId"] = id;
            IEnumerable<Product> products = await _db.Product.Where(i=>i.CategoryId==id).Include(c => c.Category).OrderByDescending(i => i.Id)
                .Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return View(products);
            //await _db.Product.Where(m => m.CategoryId == id).ToListAsync();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}