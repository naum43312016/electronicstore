using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectonicStore.Data;
using ElectonicStore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectonicStore.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ApiController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: api/<controller>
        [Route("GetAllCategories")]
        [HttpGet]
        public ActionResult Get()
        {
            IEnumerable<Category> categories = _db.Category.ToList();
            //var data = JsonConvert.SerializeObject(new { data = categories });
            return Ok(categories);
            //return Content(JsonConvert.SerializeObject(new { data = categories }), "text/javascript");
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
