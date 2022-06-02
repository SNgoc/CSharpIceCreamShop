using IceCreamClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamClient.Areas.Admin.Controllers
{
    public class MemberController : Controller
    {
        const string API_URl = "http://localhost:47255";
        IHttpClientFactory _factory;

        public MemberController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        [Area("Admin")]
        public async Task<IActionResult> Index() 
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Member/All/");
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await result.Content.ReadAsStringAsync();
                var members = JsonConvert.DeserializeObject<List<Member>>(data);
                client.Dispose();
                return View(members);
            }
            return View();
        }

        [Area("Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Member/Detail/{id}");
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await result.Content.ReadAsStringAsync();
                var member = JsonConvert.DeserializeObject<Member>(data);
                client.Dispose();
                return View(member);
            }
            return View();
        }

        [Area("Admin")]
        [HttpGet]
        public async Task<IActionResult> Deactive(string id)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Member/Deactive/{id}");
            client.Dispose();
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Success"] = "Deactive Member Successfull";
            }
            else
            {
                TempData["Error"] = "Deactive Member Failed";
            }
                return RedirectToAction("Index");
        }
        [Area("Admin")]
        [HttpGet]
        public async Task<IActionResult> Active(string id)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Member/Active/{id}");
            client.Dispose();
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Success"] = "Active Member Successfull";
            }
            else
            {
                TempData["Error"] = "Active Member Failed";
            }
            return RedirectToAction("Index");
        }

        [Area("Admin")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Member/ResetPassword/{id}");
            client.Dispose();
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Success"] = "Reset Member Password Successfully";
            }
            else
            {
                TempData["Error"] = "Reset Member Password  Failed";
            }
            return RedirectToAction("Index" );
        }

        [Area("Admin")]
        public IActionResult AddMember()
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMember(IMember imember)
        {
            if (HttpContext.Session.GetString("adname") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                if(imember.Password != imember.Password2)
                {
                    ViewData["Error"] = "Password must match to each other";
                    return View();
                }
                Member member = new Member();
                member.Username = imember.Username;
                member.Password = imember.Password;
                member.Email = imember.Email;
                member.Fullname = imember.Fullname;
                member.IdMemberOption = imember.IdMemberOption;
                HttpClient client = _factory.CreateClient();
                var memberJson = JsonConvert.SerializeObject(member);
                var stringContent = new StringContent(memberJson, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(API_URl + "/api/Member/Register", stringContent);
                client.Dispose();
                if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Success"] = "Create new member successfully";
                    return RedirectToAction("Index");
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
