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
        public async Task<ActionResult> Create(string TENSANPHAM, decimal DONGIA, int SOLUONG, HttpPostedFileBase HINHANH, string MOTA, int MADANHMUC)
        {
            List<dynamic> categories = new List<dynamic>();

            // Tải danh mục
            HttpResponseMessage productResponse = await _httpClient.GetAsync("Category");
            if (productResponse.IsSuccessStatusCode)
            {
                var category = await productResponse.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<dynamic>>(category);
            }
            else
            {
                ViewBag.ProductError = "Không thể tải dữ liệu từ API Product.";
            }

            ViewBag.Categories = categories;

            // Kiểm tra dữ liệu đầu vào
            if (HINHANH == null || string.IsNullOrEmpty(TENSANPHAM) || DONGIA <= 0 || SOLUONG <= 0 || MADANHMUC <= 0)
            {
                ViewBag.CreateError = "Dữ liệu sản phẩm không hợp lệ.";
                return View();
            }

            var fileName = Path.GetFileName(HINHANH.FileName);

            // Tạo dữ liệu sản phẩm
            var productData = new
            {
                TENSANPHAM = TENSANPHAM,
                DONGIA = DONGIA,
                SOLUONG = SOLUONG,
                MOTA = MOTA,
                MADANHMUC = MADANHMUC,
                HINHANH = fileName
            };
            var jsonContent = JsonConvert.SerializeObject(productData);
            var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // Gửi dữ liệu lên API
            HttpResponseMessage response = await _httpClient.PostAsync("Product", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                ViewBag.CreateError = $"Không thể thêm sản phẩm: {errorDetails}";
                return View();
            }
        }


        public async Task<ActionResult> Edit(int id)
        {
            List<dynamic> categories = new List<dynamic>();

            // Lấy danh sách danh mục
            HttpResponseMessage productResponse = await _httpClient.GetAsync("Category");
            if (productResponse.IsSuccessStatusCode)
            {
                var category = await productResponse.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<dynamic>>(category);
            }
            else
            {
                ViewBag.ProductError = "Không thể tải dữ liệu từ API Category.";
            }

            ViewBag.Categories = categories;

            // Lấy sản phẩm theo id
            HttpResponseMessage response = await _httpClient.GetAsync($"Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                var productData = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<dynamic>(productData);
                ViewBag.Product = product; // Thiết lập ViewBag.Product
            }
            else
            {
                ViewBag.EditError = "Không thể tải dữ liệu sản phẩm.";
                return RedirectToAction("Index"); // Quay lại Index nếu không có sản phẩm
            }

            return View(); // Trả về view nếu có sản phẩm
        }

        [HttpPost]

        public async Task<ActionResult> Edit(int MASANPHAM, string TENSANPHAM, decimal DONGIA, int SOLUONG, HttpPostedFileBase HINHANH, string MOTA, int MADANHMUC)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Product/{MASANPHAM}");
            var productData = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<dynamic>(productData);

            string fileName = null;
            if (HINHANH == null)
            {
                fileName = product.HINHANH.ToString(); 
            }
            else
            {
                fileName = Path.GetFileName(HINHANH.FileName);
            }

            // Tạo dữ liệu sản phẩm
            var productNew = new
            {
                TENSANPHAM = TENSANPHAM,
                DONGIA = DONGIA,
                SOLUONG = SOLUONG,
                MOTA = MOTA,
                MADANHMUC = MADANHMUC,
                HINHANH = fileName
            };
            var jsonContent = JsonConvert.SerializeObject(productNew);
            var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage res = await _httpClient.PutAsync($"Product/{MASANPHAM}", httpContent);
            if (res.IsSuccessStatusCode)
            {
                var products = await res.Content.ReadAsStringAsync();
                var productss = JsonConvert.DeserializeObject<dynamic>(products);
                ViewBag.Product = productss;
                return RedirectToAction("Index"); 
            }

            ViewBag.EditError = "Không thể cập nhật sản phẩm.";
            return View();  
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