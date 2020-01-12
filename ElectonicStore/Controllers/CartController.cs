using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectonicStore.Data;
using ElectonicStore.Models;
using ElectonicStore.Models.ViewModels;
using ElectonicStore.Utility;
using Microsoft.AspNetCore.Mvc;

namespace ElectonicStore.Controllers
{
    public class CartController : Controller
    {

        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<IActionResult> Index()
        {
            List<CartViewModel> cartVM = new List<CartViewModel>();
            int total = 0;
            if (SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart") != null)
            {
                List<CartModel> list = SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart");
                for(int i = 0; i < list.Count; i++)
                {
                    Product pr = await _db.Product.FindAsync(list[i].productId);
                    total += pr.Price * Int32.Parse(list[i].count);
                    CartViewModel c = new CartViewModel() { 
                        Product = pr,
                        Count = Int32.Parse(list[i].count)
                    };
                    cartVM.Add(c);
                }
            }
            ViewData["total"] = total;
            return View(cartVM);
        }


        public IActionResult Remove(int id)
        {
            if (SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart") != null)
            {
                List<CartModel> list = SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart");
                for (int i = 0; i < list.Count; i++)
                {
                    if(list[i].productId == id)
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "_Cart", list);
            }
            return RedirectToAction("Index", "Cart");
        }


        public async Task<IActionResult> Checkout()
        {
            int total = 0;
            if (SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart") != null)
            {
                List<CartModel> list = SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart");
                for (int i = 0; i < list.Count; i++)
                {
                    Product pr = await _db.Product.FindAsync(list[i].productId);
                    total += pr.Price * Int32.Parse(list[i].count);
                }
            }
            ViewData["total"] = total;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(OrderDetails order)
        {
            if (SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart") != null)
            {
                await _db.OrderDetails.AddAsync(order);
                await _db.SaveChangesAsync();
                
                List<CartModel> list = SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart");
                for (int i = 0; i < list.Count; i++)
                {
                    OrderProducts orPr = new OrderProducts()
                    {
                        ProductId = list[i].productId,
                        Count = Int32.Parse(list[i].count),
                        OrderId = order.Id
                    };
                    await _db.OrderProducts.AddAsync(orPr);
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Success", "Cart", new { id=order.Id});
        }

        public IActionResult Success(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        [HttpPost]
        public IActionResult Add(int id)
        {
            return View();
        }
    }
}