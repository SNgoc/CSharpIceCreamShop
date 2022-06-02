using IceCreamClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamClient.Controllers
{
    public class MemberController : Controller
    {
        const string API_URl = "http://localhost:47255";
        IHttpClientFactory _factory;

        public MemberController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = _factory.CreateClient();
                var result = await client.GetAsync(API_URl + $"/api/Member/Login/{Username}/{Password}");
                if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = await result.Content.ReadAsStringAsync();
                    var member = JsonConvert.DeserializeObject<Member>(data);
                    HttpContext.Session.SetString("user_detail", JsonConvert.SerializeObject(member));
                    client.Dispose();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["errorLogin"] = "wrong username or password";
                }
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Member member)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = _factory.CreateClient();
                var memberJson = JsonConvert.SerializeObject(member);
                var stringContent = new StringContent(memberJson, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(API_URl + "/api/Member/Register", stringContent);
                client.Dispose();
                if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Member/Detail/{id}");
            client.Dispose();
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await result.Content.ReadAsStringAsync();
                var member = JsonConvert.DeserializeObject<Member>(data);
                client.Dispose();
                return View(member);
            }
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)

        {
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Member/Detail/{id}");
            client.Dispose();
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await result.Content.ReadAsStringAsync();
                var member = JsonConvert.DeserializeObject<Member>(data);
                client.Dispose();
                return View(member);
            }
            return View("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Member member)
        {
            HttpClient client = _factory.CreateClient();
            var memberJson = JsonConvert.SerializeObject(member);
            var stringContent = new StringContent(memberJson, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(API_URl + $"/api/Member/Update", stringContent);
            client.Dispose();
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await result.Content.ReadAsStringAsync();
                var mem = JsonConvert.DeserializeObject<Member>(data);
                return View("Detail",mem);
            }
            else
            {
                return View("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Member/Detail/{id}");
            client.Dispose();
            if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await result.Content.ReadAsStringAsync();
                var member = JsonConvert.DeserializeObject<Member>(data);
                client.Dispose();
                return View(member);
            }
            return NoContent();
        }

        //view data khoong chay

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string id, string OldPassword, string NewPassword1, string NewPassword2)
        {
            if (NewPassword1 != null && NewPassword2 != null && NewPassword1 == NewPassword2)
            {
                HttpClient client = _factory.CreateClient();
                var result = await client.GetAsync(API_URl + $"/api/Member/Password/{id}/{OldPassword}/{NewPassword1}");
                client.Dispose();
                if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    client.Dispose();
                    TempData["ChangePassSuccess"] = "Change password successfully";
                    return RedirectToAction("ChangePassword", "Member", id);
                }
                else
                {
                    TempData["ChangePassError"] = "Old Password doesn't correct";
                    return RedirectToAction("ChangePassword", "Member", id);
                }
            }
            else
            {
                TempData["ChangePassError"] = "New Password must match to each other";
                return RedirectToAction("ChangePassword", "Member", id);
            }

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user_detail");
            return RedirectToAction("Index","Home");
        }
    }
}
