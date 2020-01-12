using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectonicStore.Data;
using ElectonicStore.Models;
using ElectonicStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectonicStore.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> list = await _db.Category.ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(category.Name.Length > 0)
            {
                await _db.Category.AddAsync(category);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Category");
            }
            return View();
        }


        
        
        public IActionResult Delete(int id)
        {
            if (id!= 0)
            {
                var cat = _db.Category.Find(id);
                if (cat != null)
                {
                    _db.Category.Remove(cat);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Category");
                }
            }
            return NotFound();
        }

        public IActionResult update(int id)
        {
            var cat = _db.Category.Find(id);
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult update(Category cat)
        {
            if (cat == null)
            {
                return NotFound();
            }
            _db.Update(cat);
            _db.SaveChanges();
            return RedirectToAction("Index", "Category");
        }

    }
}