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
    public class ProductDetailsController : Controller
    {

        private readonly ApplicationDbContext _db;

        public ProductDetailsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Details(int id)
        {
            Product product = await _db.Product.Include(c => c.Category).Where(i => i.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            char[] spearator = { '*' };
            List<string> images = product.Images.Split(spearator,
               StringSplitOptions.None).ToList();
            ViewData["images"] = images;
            return View(product);
        }
    }
}