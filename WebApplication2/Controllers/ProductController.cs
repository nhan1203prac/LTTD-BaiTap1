using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication2.Controllers
{
    public class ProductController : ApiController
    {
        dbQUANLYBANHANGEntities db = new dbQUANLYBANHANGEntities();
        [HttpGet]
        [Route("api/Product/{id}")]
        public IHttpActionResult GetProductById(int id)
        {
            var product = db.tbSANPHAMs
                            .Where(s => s.MASANPHAM == id)
                            .Select(s => new
                            {
                                s.MASANPHAM,
                                s.TENSANPHAM,
                                s.DONGIA,
                                s.SOLUONG,
                                s.HINHANH,
                                s.MOTA,
                                s.MADANHMUC
                            })
                            .FirstOrDefault();

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet]
        [Route("api/Product")]
        public IHttpActionResult GetProducts()
        {
            var product = db.tbSANPHAMs.Select(s => new
            {
                s.MASANPHAM,
                s.TENSANPHAM,
                s.DONGIA,
                s.SOLUONG,
                s.HINHANH,
                s.MOTA,
                s.MADANHMUC
            }).ToList();
                            
            if (product == null)
                return NotFound();
            return Ok(product);
        }
        [HttpGet]
        [Route("api/Product/category/{id}")]
        public IHttpActionResult GetProductByCategory(int id)
        {
            var products = db.tbSANPHAMs
                             .Where(s => s.MADANHMUC == id)
                             .Select(s => new
                             {
                                 s.MASANPHAM,
                                 s.TENSANPHAM,
                                 s.DONGIA,
                                 s.SOLUONG,
                                 s.HINHANH,
                                 s.MOTA,
                                 s.MADANHMUC
                             })
                             .ToList(); // Get all matching products as a list

            return Ok(products); // Return the list (empty or populated)
        }
        [HttpPost]
        [Route("api/Product")]
        public IHttpActionResult CreateProduct([FromBody] tbSANPHAM newProduct)
        {
            if (newProduct == null)
                return BadRequest("Invalid product data.");

            db.tbSANPHAMs.Add(newProduct);
            db.SaveChanges();

            return Ok("Created successfully");
        }

        [HttpPut]
        [Route("api/Product/{id:int}")]
        public IHttpActionResult UpdateProduct([FromBody] tbSANPHAM updatedProduct)
        {
            if (updatedProduct == null)
                return BadRequest("Invalid product data.");

            var existingProduct = db.tbSANPHAMs.Find(updatedProduct.MASANPHAM);
            if (existingProduct == null)
                return NotFound();

            existingProduct.TENSANPHAM = updatedProduct.TENSANPHAM;
            existingProduct.DONGIA = updatedProduct.DONGIA;
            existingProduct.SOLUONG = updatedProduct.SOLUONG;
            existingProduct.HINHANH = updatedProduct.HINHANH;
            existingProduct.MOTA = updatedProduct.MOTA;
            existingProduct.MADANHMUC = updatedProduct.MADANHMUC;

            db.SaveChanges();

            return Ok(existingProduct);
        }

        [HttpDelete]
        [Route("api/Product/{id:int}")]
        public IHttpActionResult DeleteProduct(int id)
        {
            var product = db.tbSANPHAMs.Find(id);
            if (product == null)
                return NotFound();

            db.tbSANPHAMs.Remove(product);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

    }
}
