using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectonicStore.Data;
using ElectonicStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectonicStore.Controllers
{
    public class SearchController : Controller
    {

        private readonly ApplicationDbContext _db;

        public SearchController( ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchText,string catid,int page)
        {
            int perPage = 3;
            if (page == 0) page = 1;
            int categoryId = Int32.Parse(catid);
            if (categoryId == 0)
            {
                //All categories
                IEnumerable<Product> products = await _db.Product.Where(s=>s.Name.Contains(searchText)).Include(c => c.Category).OrderByDescending(i => i.Id)
                .Skip((page - 1) * perPage).Take(perPage).ToListAsync();
                int count = products.Count();
                int pagesCount = (int)Math.Ceiling((count + 0.0) / (perPage + 0.0));
                if (page > pagesCount) page = 1;
                ViewData["currentPage"] = page;
                ViewData["pagesCount"] = pagesCount;
                ViewData["categoryId"] = categoryId;
                return View(products);
            }
            else
            {
                IEnumerable<Product> products = await _db.Product.Where(c=>c.CategoryId== categoryId).Where(s => s.Name.Contains(searchText)).Include(c => c.Category).OrderByDescending(i => i.Id)
                .Skip((page - 1) * perPage).Take(perPage).ToListAsync();
                int count = products.Count();
                int pagesCount = (int)Math.Ceiling((count + 0.0) / (perPage + 0.0));
                if (page > pagesCount) page = 1;
                ViewData["currentPage"] = page;
                ViewData["pagesCount"] = pagesCount;
                ViewData["categoryId"] = categoryId;
                return View(products);
            }
        }
    }
}