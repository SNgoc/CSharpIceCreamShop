using IceCreamClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace IceCreamClient.Controllers
{
    public class RecipeController : Controller
    {
        const String BASE_URL = "http://localhost:47255";
        HttpClient _client;
        IHttpContextAccessor _httpContextAccessor;

        public RecipeController(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _client.BaseAddress = new Uri(BASE_URL);
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(int? CategoryID, int? page)
        {
            const int LIMIT = 6;
            IPagedList<Recipe> onePageofItem;
            if (CategoryID == null)
            {
                var response = await _client.GetAsync("/api/recipe/active");
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Recipe>>(data);

                int totalItem = result.Count();
                int totalPage = totalItem / LIMIT + 1;
                
                var pageNumber = page ?? 1;
                onePageofItem = result.ToPagedList(pageNumber, LIMIT);

                TempData["TotalPage"] = totalPage;
                TempData["CurrentPage"] = page;


            }
            else
            {
                var response = await _client.GetAsync($"/api/recipe/category/{CategoryID}");
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Recipe>>(data);

                int totalItem = result.Count();
                int totalPage = totalItem / LIMIT + 1;

                var pageNumber = page ?? 1;
                onePageofItem = result.ToPagedList(pageNumber, LIMIT);

                TempData["CategoryID"] = CategoryID;
                TempData["TotalPage"] = totalPage;
                TempData["CurrentPage"] = page;
            }

            _client.Dispose();
            return View(onePageofItem);
        }

        public async Task<IActionResult> Details(int RecipeId)
        {
            var responseDetails = await _client.GetAsync($"/api/recipe/{RecipeId}");
            var detailsData = await responseDetails.Content.ReadAsStringAsync();
            var details = JsonConvert.DeserializeObject<Recipe>(detailsData);

            if (details.PayingRequired == true)
            {
                string json_user_detail = _httpContextAccessor.HttpContext.Session.GetString("user_detail");
                if (json_user_detail == null)
                {
                    return RedirectToAction("Login", "Member");
                }
            }

            var responseLatest = await _client.GetAsync($"/api/recipe/latest/{details.CategoryID}/3");
            var latestData = await responseLatest.Content.ReadAsStringAsync();
            var latest = JsonConvert.DeserializeObject<List<Recipe>>(latestData);

            var responseComment = await _client.GetAsync($"/api/comment/byRecipe/{RecipeId}");
            if (responseComment.IsSuccessStatusCode)
            {
                var commentData = await responseComment.Content.ReadAsStringAsync();
                var comment = JsonConvert.DeserializeObject<List<Comment>>(commentData);
                ViewData["Comment"] = comment;
            }

            _client.Dispose();

            ViewData["Details"] = details;
            ViewData["Latest"] = latest;          

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Comment(Comment comment)
        {

            comment.isReplied = false;

            var json = JsonConvert.SerializeObject(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/comment", content);

            _client.Dispose();
            if (response.IsSuccessStatusCode)
            {
                TempData["CommentSuccess"] = "Comment success";
                return RedirectToAction("Details", new { RecipeId = comment.RecipeId });
            }

            TempData["CommentError"] = "Comment error !";
            return RedirectToAction("Details", new { RecipeId = comment.RecipeId });
        }
    }
}
