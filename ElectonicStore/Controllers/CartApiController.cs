using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectonicStore.Models;
using ElectonicStore.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectonicStore.Controllers
{
    [Route("api/AddToCart")]
    [ApiController]
    public class CartApiController : ControllerBase
    {

        [Route("add")]
        [HttpPost]
        public bool Add([FromBody]CartModel obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                /*List<CartModel> list = new List<CartModel>();
            CartModel c1 = new CartModel() { 
                productId =1,
                count=3
            };
            list.Add(c1);
            SessionHelper.SetObjectAsJson(HttpContext.Session,"_Cart",list);

            List<CartModel> listFromSes = SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart");
            return Content("Product Id= " + listFromSes[0].productId+" Count="+ listFromSes[0].count);*/
                if (SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart")==null)
                {
                    List<CartModel> list = new List<CartModel>();
                    CartModel c1 = new CartModel()
                    {
                        productId = obj.productId,
                        count = obj.count
                    };
                    list.Add(c1);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "_Cart", list);
                }
                else
                {
                    List<CartModel> list = SessionHelper.GetObjectFromJson<List<CartModel>>(HttpContext.Session, "_Cart");
                    for(int i = 0; i < list.Count; i++)
                    {
                        if(list[i].productId==obj.productId)
                        {
                            list[i].count += obj.count;
                            SessionHelper.SetObjectAsJson(HttpContext.Session, "_Cart", list);
                            return true;
                        }
                    }
                    CartModel c1 = new CartModel()
                    {
                        productId = obj.productId,
                        count = obj.count
                    };
                    list.Add(c1);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "_Cart", list);
                }
                return true;
            }
        }

    }
}