using IceCreamClient.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RecipeController : Controller
    {
        const String BASE_URL = "http://localhost:47255";
        HttpClient _client;

        public RecipeController(HttpClient client)
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
            var response = await _client.GetAsync("/api/recipe");
            //if (response.IsSuccessStatusCode)
            //{
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Recipe>>(data);

            _client.Dispose();
                return View(result);
            //}
            //return RedirectToAction("ErrorPage");
        }

        public async Task<IActionResult> Details(int RecipeId)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var response = await _client.GetAsync($"/api/recipe/{RecipeId}");
            //if (response.IsSuccessStatusCode)
            //{
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Recipe>(data);

            _client.Dispose();
            return View(result);
            //}
            //return RedirectToAction("ErrorPage");
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Recipe recipe, IFormFile imageFile)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (imageFile != null)
            {
                var filepath = Path.Combine("wwwroot/images/recipe/", imageFile.FileName);
                Stream stream = new FileStream(filepath, FileMode.Create);
                await imageFile.CopyToAsync(stream);
                recipe.Thumbnail = imageFile.FileName;
                stream.Close();
            }
            else
            {
                recipe.Thumbnail = "image_not_found.png";
            }
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(recipe);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("/api/recipe", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Update success";
                    return RedirectToAction("Index");
                }
            }
            TempData["Error"] = "Input error !";

            _client.Dispose();
            return View();
        }

        public async Task<IActionResult> Edit(int RecipeId)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var response = await _client.GetAsync($"/api/recipe/{RecipeId}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Recipe>(data);
                return View(result);
            }
            TempData["Error"] = "Server error. Cannot fetch data !";

            _client.Dispose();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int RecipeId, Recipe recipe, IFormFile imageFile)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (imageFile != null)
            {
                var filepath = Path.Combine("wwwroot/images/recipe/", imageFile.FileName);
                Stream stream = new FileStream(filepath, FileMode.Create);
                await imageFile.CopyToAsync(stream);
                recipe.Thumbnail = "~/images/recipe/" + imageFile.FileName;
                stream.Close();
            }
            else
            {
                recipe.Thumbnail = "";
            }
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(recipe);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"/api/recipe/{RecipeId}", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Update success";
                    return RedirectToAction("Index");
                }
            }
            TempData["Error"] = "Input error !";

            _client.Dispose();
            return RedirectToAction("Edit");
        }

        public async Task<IActionResult> UpdateStatus(int RecipeId, bool status)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var content = new StringContent("", Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/recipe/status/{RecipeId}/{status}", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Update success";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Input error !";

            _client.Dispose();
            return RedirectToAction("Index"); ;
        }

        public async Task<IActionResult> UpdatePayingStatus(int RecipeId, bool payingStatus)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var content = new StringContent("", Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/recipe/payingRequired/{RecipeId}/{payingStatus}", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Update success";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Input error !";

            _client.Dispose();
            return RedirectToAction("Index");
        }
    }
}
