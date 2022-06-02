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
    [Area("Admin")]//thêm route /admin/book cho trang admin
    public class CategoryController : Controller
    {
        const string BASE_URL = "http://localhost:47255";
        IHttpClientFactory factory;

        public CategoryController(IHttpClientFactory factory)
        {
            this.factory = factory;
        }

        //view category
        public async Task<IActionResult> Index()
        {
            //check admin login
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpClient client = factory.CreateClient();//tạo và nhận data
            var result = await client.GetStringAsync(BASE_URL + "/api/category");
            var cate = JsonConvert.DeserializeObject<List<Category>>(result);
            client.Dispose();
            return View(cate);//trả về view obj cate hiện tại để hiển thị thông tin book, ko có sẽ bị lỗi null model khi show list bên index.cshtml
        }
        //end view cate

        //create cate
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category cate)
        {
            //check admin login
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)//validate
            {
                HttpClient client = factory.CreateClient();//tạo và nhận data

                //create cate
                var cateJson = JsonConvert.SerializeObject(cate);
                var stringContent = new StringContent(cateJson, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(BASE_URL + "/api/category/", stringContent);
                client.Dispose();
                return RedirectToAction("Index");
            }
            return View();
        }
        //end create

        //Update category
        public async Task<IActionResult> UpdateCategory(int CatId) // CatId phải giống với link CatId = item.CatId bên Index.cshtml
        {
            //check admin login
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpClient client = factory.CreateClient();
            var response = await client.GetStringAsync(BASE_URL + $"/api/category/{CatId}");
            var cate = JsonConvert.DeserializeObject<Category>(response);
            client.Dispose();
            return View(cate);//trả về view obj cate hiện tại để hiển thị thông tin sau edit, ko có sẽ bị lỗi null model khi show list bên UpdateCategory.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(int CatId, Category cate)
        {
            if (ModelState.IsValid)//validate
            {
                HttpClient client = factory.CreateClient();
                var json = JsonConvert.SerializeObject(cate);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await client.PutAsync(BASE_URL + "/api/category", stringContent);//put nếu bên api cũng dùng put
                if (result.IsSuccessStatusCode)
                {
                    ViewData["success"] = "Updated Book successful.";
                }
                client.Dispose();
                return View(cate);
            }
            return View(cate);//trả về view obj cate hiện tại để hiển thị thông tin sau edit, ko có sẽ bị lỗi null model khi update failed
        }
        //END UPDATE
    }
}

