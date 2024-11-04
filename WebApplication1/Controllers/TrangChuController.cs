using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly HttpClient _httpClient;

        public TrangChuController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44308/api/")
            };
        }

      
        public async Task<ActionResult> Index()
        {
            List<dynamic> categories = new List<dynamic>();
            List<dynamic> products = new List<dynamic>();

            // Gửi yêu cầu đến API /Category
            HttpResponseMessage categoryResponse = await _httpClient.GetAsync("Category");
            if (categoryResponse.IsSuccessStatusCode)
            {
                var jsonData = await categoryResponse.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<dynamic>>(jsonData);
            }
            else
            {
                ViewBag.CategoryError = "Không thể tải dữ liệu từ API Category.";
            }

            // Gửi yêu cầu đến API /Product
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

            // Chuyển cả hai danh sách đến view bằng ViewBag
            ViewBag.Categories = categories;
            ViewBag.Products = products;

            return View();
        }

        public async Task<ActionResult> ProductDetails(int id)
        {
            dynamic product = null;

            List<dynamic> categories = new List<dynamic>();
            

            // Gửi yêu cầu đến API /Category
            HttpResponseMessage categoryResponse = await _httpClient.GetAsync("Category");
            if (categoryResponse.IsSuccessStatusCode)
            {
                var jsonData = await categoryResponse.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<dynamic>>(jsonData);
            }
            else
            {
                ViewBag.CategoryError = "Không thể tải dữ liệu từ API Category.";
            }

            // Gửi yêu cầu đến API /Product/{id}
            HttpResponseMessage productResponse = await _httpClient.GetAsync($"Product/{id}");
            if (productResponse.IsSuccessStatusCode)
            {
                var productData = await productResponse.Content.ReadAsStringAsync();
                product = JsonConvert.DeserializeObject<dynamic>(productData);
            }
            else
            {
                ViewBag.ProductError = "Không thể tải dữ liệu từ API cho sản phẩm này.";
            }

            // Chuyển thông tin sản phẩm đến view bằng ViewBag
            ViewBag.Product = product;
            ViewBag.Categories = categories;
            return View();
        }

    }
}
