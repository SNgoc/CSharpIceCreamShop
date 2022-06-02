using IceCreamClient.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        const string API_URl = "http://localhost:47255";
        IHttpClientFactory _factory;

        public HomeController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return RedirectToAction("Index", "Book");
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("adname") != null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(tbAdmin admin)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = _factory.CreateClient();
                var memberJson = JsonConvert.SerializeObject(admin);
                var stringContent = new StringContent(memberJson, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(API_URl + "/api/Admin/Login", stringContent);
                client.Dispose();
                if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = await result.Content.ReadAsStringAsync();
                    var ad = JsonConvert.DeserializeObject<tbAdmin>(data);
                    HttpContext.Session.SetInt32("id_ad", ad.Id);
                    HttpContext.Session.SetString("adname", ad.Username);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["Error"] = "Wrong username or password!";
                }
            }
            return View();
        }
        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldpass, string newpass1, string newpass2)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (newpass1 != newpass2)
            {
                ViewData["Error"] = "New password must the same with each other!";
                return View();
            }
            string id = (HttpContext.Session.GetInt32("id_ad")).ToString();
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Admin/Password/{id}/{oldpass}/{newpass1}");
            client.Dispose();
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViewData["Success"] = "Change Admin Password Successfully";
                var data = await result.Content.ReadAsStringAsync();
                var admin = JsonConvert.DeserializeObject<tbAdmin>(data);
                client.Dispose();

                return View(admin);
            }
            ViewData["Error"] = "Change Admin Password Failed, wrong old password";

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("adname");
            HttpContext.Session.Remove("id_ad");
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ViewAll()
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + "/api/Admin/All");
            client.Dispose();
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await result.Content.ReadAsStringAsync();
                var ads = JsonConvert.DeserializeObject<List<tbAdmin>>(data);
                return View(ads);
            }
            else
            {
               return NotFound();
            }
        }
        public IActionResult AddAdmin()
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAdmin(ITbAdmin iadmin)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                if(iadmin.Password != iadmin.Password2)
                {
                    ViewData["Error"] = "Password must match to each other!";
                    return View();
                }
                tbAdmin admin = new tbAdmin();
                admin.Username = iadmin.Username;
                admin.Password = iadmin.Password;
                admin.Username = iadmin.Username;
                  HttpClient client = _factory.CreateClient();
                var adminJson = JsonConvert.SerializeObject(admin);
                var stringContent = new StringContent(adminJson, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(API_URl + "/api/Admin/Add", stringContent);
                client.Dispose();
                if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Success"] = "Create new Admin successfully";
                    return RedirectToAction("ViewAll");
                }
                else
                {
                    ViewData["Error"] = "Username already exists!";
                }
            }

            return View();
        }
    }
}
