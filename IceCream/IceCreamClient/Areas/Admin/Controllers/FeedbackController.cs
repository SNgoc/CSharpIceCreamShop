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
    public class FeedbackController : Controller
    {
        const string API_URl = "http://localhost:47255";
        IHttpClientFactory _factory;

        public FeedbackController(IHttpClientFactory factory)
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
            var result = await client.GetAsync(API_URl + $"/api/Feedback/All");
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await result.Content.ReadAsStringAsync();
                var fbs = JsonConvert.DeserializeObject<List<Feedback>>(data);
                client.Dispose();
                return View(fbs);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Feedback/Detail/{id}");
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await result.Content.ReadAsStringAsync();
                var fb = JsonConvert.DeserializeObject<Feedback>(data);
                client.Dispose();
                return View(fb);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpClient client = _factory.CreateClient();
            var result = await client.DeleteAsync(API_URl + $"/api/Feedback/{id}");
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Success"] = "Delete Feedback Successfully";
            }
            else
            {
                TempData["Error"] = "Delete Feedback Failed";
            }
            return RedirectToAction("Index");
        }
    }
}
