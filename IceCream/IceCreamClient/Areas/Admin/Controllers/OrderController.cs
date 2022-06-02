using IceCreamClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IceCreamClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        const string API_URl = "http://localhost:47255";
        IHttpClientFactory _factory;

        public OrderController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Order/AllOrder/");
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await result.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<BookOrder>>(data);
                client.Dispose();
                return View(orders);
            }
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Order/GetOrder/{id}");
            var data = await result.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<BookOrder>(data);
            ViewBag.Order = order;

            var result2 = await client.GetAsync(API_URl + $"/api/Order/OrderDetail/{id}");
            var data2 = await result2.Content.ReadAsStringAsync();
            var details = JsonConvert.DeserializeObject<List<OrderDetailService>>(data2);           
            client.Dispose();
            return View("Details", details);
        }

        public async Task<IActionResult> Completed(int id)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Order/Completed/{id}");
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Success"] = "Change Status sussessfully";
            }
            else
            {
                TempData["Error"] = "Change Status Failed";
            }
            client.Dispose();           
            return RedirectToAction("Index");
        }
    }
}
