using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication2.Controllers
{
    [Route("api/Category")]
    public class CategoryController : ApiController
    {

        dbQUANLYBANHANGEntities db = new dbQUANLYBANHANGEntities();


        [HttpGet]
        
        public IHttpActionResult GetCategories()
        {
            var category = db.tbDANHMUCs.Select(s => new
            {
                s.MADANHMUC,
                s.TENDANHMUC,
                s.DANHMUCCHA,
                s.MOTA,
                
            }).ToList();

            if (category == null)
                return NotFound();
            return Ok(category);
        }



    }
}
