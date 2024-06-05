using Microsoft.AspNetCore.Mvc;
using MSIT158_2_API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSIT158_2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class F31 : ControllerBase
    {
        // GET: api/<F31>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<F31>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<F31>
        [HttpPost]
        public void Post(TOrder value)
        {
            SelectShopContext db = new SelectShopContext();
            db.TOrders.Add(value);
            db.SaveChanges();
            int lo = db.TOrders.OrderBy(x => x.OrderId).FirstOrDefault().OrderId;
            var a = db.TCarts.Where(x => x.MemberId == value.MemberId);
            foreach (var x in a)
            {
                TPurchase p = new TPurchase();
                p.OrderId = lo;
                p.ProductId = x.ProductId;
                p.Qty = x.Qty;
                db.TPurchases.Add(p);
                db.TCarts.Remove(x);
            }
            db.SaveChanges();
        }

        // PUT api/<F31>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<F31>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
