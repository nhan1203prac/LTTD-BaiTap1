using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44308/api/")
            };
        }
        // GET: Admin
        public async Task<ActionResult> Index()
        {
           
            List<dynamic> products = new List<dynamic>();

            HttpResponseMessage productResponse = await _httpClient.GetAsync("Product");
            if (productResponse.IsSuccessStatusCode)
            {
                var productData = await productResponse.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<dynamic>>(productData);
            }
            else
            {
                ViewBag.ProductError = "Không thể tải dữ liệu từ API Product.";
            }

          
            
            ViewBag.Products = products;

            return View();
        }


     
        public async Task<ActionResult> Create()
        {

            List<dynamic> categories = new List<dynamic>();

            HttpResponseMessage productResponse = await _httpClient.GetAsync("Category");
            if (productResponse.IsSuccessStatusCode)
            {
                var productData = await productResponse.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<dynamic>>(productData);
            }
            else
            {
                ViewBag.ProductError = "Không thể tải dữ liệu từ API Product.";
            }



            ViewBag.Categories = categories;
            return View(); 
        }

        [HttpPost]
        public async Task<ActionResult> Create(int MASANPHAM, string TENSANPHAM, decimal DONGIA, int SOLUONG, HttpPostedFileBase HINHANH, string MOTA, int MADANHMUC)


        {

            var fileName = Path.GetFileName(HINHANH.FileName);
            if (MASANPHAM==null || TENSANPHAM == null || DONGIA == null || SOLUONG == null || HINHANH == null || MOTA == null || MADANHMUC == null)
            {
                ViewBag.CreateError = "Dữ liệu sản phẩm không hợp lệ.";
                return View();
            }
            var productData = new
            {
                MASANPHAM = MASANPHAM,
                TENSANPHAM = TENSANPHAM,
                DONGIA = DONGIA,
                SOLUONG = SOLUONG,
                MOTA = MOTA,
                MADANHMUC = MADANHMUC,
                HINHANH = fileName // Lưu tên tệp hình ảnh
            };
            var jsonContent = JsonConvert.SerializeObject(productData);
            var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("Product", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            //
            


          

            ViewBag.CreateError = "Không thể thêm sản phẩm.";
            return View();
        }


        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                var productData = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<dynamic>(productData);
                return View(product); 
            }

            ViewBag.EditError = "Không thể tải dữ liệu sản phẩm.";
            return RedirectToAction("Index");
        }

        [HttpPut]

        public async Task<ActionResult> Edit(int id, dynamic product)
        {
            var jsonContent = JsonConvert.SerializeObject(product);
            var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"Product/{id}", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); 
            }

            ViewBag.EditError = "Không thể cập nhật sản phẩm.";
            return View(product);
        }

     
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                var productData = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<dynamic>(productData);
                return View(product); 
            }

            ViewBag.DeleteError = "Không thể tải dữ liệu sản phẩm.";
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); 
            }

            ViewBag.DeleteError = "Không thể xóa sản phẩm.";
            return RedirectToAction("Delete", new { id });
        }

    }
}