using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectonicStore.Data;
using ElectonicStore.Models;
using ElectonicStore.Models.ViewModels;
using ElectonicStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectonicStore.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> list = _db.Product.Include(m => m.Category).AsEnumerable();
            return View(list);
        }


        public async Task<IActionResult> Create()
        {
            ProductCreateViewModel prVm = new ProductCreateViewModel();
            prVm.product = new Product();
            prVm.Categories = await _db.Category.ToListAsync();
            return View(prVm);
        }


        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _db.Product.FindAsync(id);
            _db.Product.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("index", "product");
        }


        public async Task<IActionResult> Update(int id)
        {
            Product product = await _db.Product.Include(o => o.Category).Where(o => o.Id == id).FirstOrDefaultAsync();
            IEnumerable<Category> categories = await _db.Category.ToListAsync();
            ViewData["categories"] = categories;
            char[] spearator = { '*'};
            List<string> images = product.Images.Split(spearator,
               StringSplitOptions.None).ToList();
            ViewData["images"] = images;
            ViewData["imagesString"] = product.Images;

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product product, IFormFile[] photos)
        {
            var prFromDb = await _db.Product.FindAsync(product.Id);
            char[] spearator = {'*'};
            String[] strlist = product.Images.Split(spearator,
               StringSplitOptions.None);
            StringBuilder str = new StringBuilder();
            for(int i = 0; i < strlist.Length; i++)
            {
                str.Append(strlist[i]);
                str.Append("*");
            }
            if (photos != null || photos.Length > 0)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                foreach (IFormFile photo in photos)
                {
                    var path = Path.Combine(webRootPath, "images", photo.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    await photo.CopyToAsync(stream);
                    str.Append("images/" + photo.FileName);
                    str.Append("*");
                }
            }
            
            
            product.Images = str.ToString();
            product.Category = await _db.Category.FindAsync(product.CategoryId);


            prFromDb.Images = product.Images;
            prFromDb.Category = product.Category;
            prFromDb.CategoryId = product.CategoryId;
            prFromDb.Name = product.Name;
            prFromDb.Quantity = product.Quantity;
            prFromDb.Price = product.Price;


            await _db.SaveChangesAsync();

            return RedirectToAction("index", "product");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel productVm,IFormFile[] photos)
        {
            StringBuilder str = new StringBuilder();
            // *
            if (photos != null || photos.Length > 0)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                foreach (IFormFile photo in photos)
                {
                    var path = Path.Combine(webRootPath, "images", photo.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    await photo.CopyToAsync(stream);
                    str.Append("images/"+photo.FileName);
                    str.Append("*");
                }
            }
            Product pr = new Product();
            pr = productVm.product;
            pr.Images = str.ToString();
            pr.Category = await _db.Category.FindAsync(pr.CategoryId);
            await _db.Product.AddAsync(pr);
            await _db.SaveChangesAsync();

            /*char[] spearator = { '*'};
            String[] strlist = strin.Split(spearator,
               StringSplitOptions.None);*/
            /*return Content(
                "Name=" + product.Name + " Price =" + product.Price + " Quantity=" + product.Quantity
                + " Images=" +str + " strlist1="+ strlist[0] + " strlist1=" + strlist[1]);*/
            return RedirectToAction("index", "product");
        }
    }
}