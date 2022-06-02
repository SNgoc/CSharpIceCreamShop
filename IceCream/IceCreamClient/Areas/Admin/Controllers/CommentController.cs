using IceCreamClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        const String BASE_URL = "http://localhost:47255";
        HttpClient _client;

        public CommentController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(BASE_URL);
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var response = await _client.GetAsync("/api/comment");
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Comment>>(data);

            _client.Dispose();
            return View(result);
        }

        public async Task<IActionResult> Reply(int id)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var response = await _client.GetAsync($"/api/comment/{id}");
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Comment>(data);

            _client.Dispose();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Reply(int id, String reply)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var comment = new Comment();
            comment.Reply = reply;
            var json = JsonConvert.SerializeObject(comment);
            var contentStr = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/comment/{id}", contentStr);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Reply success";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Input error !";

            _client.Dispose();
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var response = await _client.DeleteAsync($"/api/comment/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Delete success";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Delete error !";

            _client.Dispose();
            return RedirectToAction("Index");
        }
    }
}
