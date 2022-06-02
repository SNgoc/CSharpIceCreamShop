using IceCreamClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamClient.Controllers
{
    public class ContactController : Controller
    {
        const string API_URl = "http://localhost:47255";
        IHttpClientFactory _factory;

        public ContactController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Feedback fb)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = _factory.CreateClient();
                var fbJson = JsonConvert.SerializeObject(fb);
                var stringContent = new StringContent(fbJson, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(API_URl + "/api/Feedback", stringContent);
                client.Dispose();
                if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewData["Feedback"] = "Submit feedback successfully";
                    return View("Index");
                }
            }
            ViewData["Feedback"] = "Submit feedback error";
            return View();
        }
    }
}
