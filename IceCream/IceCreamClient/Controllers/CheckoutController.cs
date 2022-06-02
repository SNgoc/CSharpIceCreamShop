using IceCreamClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamClient.Controllers
{
    public class CheckoutController : Controller
    {
        const string API_URl = "http://localhost:47255";
        IHttpClientFactory _factory;
        IHttpContextAccessor _HttpContextAccessor;

        public CheckoutController(IHttpClientFactory factory, IHttpContextAccessor HttpContextAccessor)
        {
            _factory = factory;
            _HttpContextAccessor = HttpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(BookOrder order)
        {
            string json = _HttpContextAccessor.HttpContext.Session.GetString("cart");
            List<Cart> listCart = new List<Cart>();
            double? total_money = 0;
            if (json != null)
            {
                JArray jsonResponse = JArray.Parse(json);

                foreach (var item in jsonResponse)
                {
                    JObject cartResult = (JObject)item;
                    listCart.Add(JsonConvert.DeserializeObject<Cart>(cartResult.ToString()));
                }
                foreach (Cart cart in listCart)
                {
                    total_money += cart.Quantity * cart.Price;
                }
                order.TotalPrice = Convert.ToInt32(total_money);
            }


            if (ModelState.IsValid)
            {
                HttpClient client = _factory.CreateClient();
                var memberJson = JsonConvert.SerializeObject(order);
                var stringContent = new StringContent(memberJson, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(API_URl + $"/api/Order/", stringContent);
                if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = await result.Content.ReadAsStringAsync();
                    var id = JsonConvert.DeserializeObject<int>(data);
                    List<BookOrderDetail> details = new List<BookOrderDetail>();
                    if (listCart.Count > 0)
                    {
                        foreach (Cart cart in listCart)
                        {
                            details.Add(new BookOrderDetail
                            {
                                BookId = cart.BookIceCream.BookId,
                                Price = Convert.ToInt32(cart.Price),
                                BookOrderId = id,
                                Quantity = cart.Quantity
                            });
                            total_money += cart.Quantity * cart.Price;
                        }
                        order.TotalPrice = Convert.ToInt32(total_money);
                    }

                    var detailsJSON = JsonConvert.SerializeObject(details);
                    var stringContent2 = new StringContent(detailsJSON, Encoding.UTF8, "application/json");
                    var result2 = await client.PostAsync(API_URl + $"/api/Order/Detail", stringContent2);
                    client.Dispose();
                    if (result2.IsSuccessStatusCode && result2.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View("Index");
        }

        public IActionResult CheckOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckOrder(string phone)
        {

            HttpClient client = _factory.CreateClient();
            var resultID = await client.GetAsync(API_URl + $"/api/Order/Phone/{phone}");
            if (resultID.IsSuccessStatusCode && resultID.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = await resultID.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<BookOrder>(data);
                ViewBag.Order = order;

                var result2 = await client.GetAsync(API_URl + $"/api/Order/OrderDetail/{order.Id}");
                var data2 = await result2.Content.ReadAsStringAsync();
                var details = JsonConvert.DeserializeObject<List<OrderDetailService>>(data2);
                ViewBag.Details = details;
                client.Dispose();
                return View();
            }
            else
            {
                ViewData["Error"] = "The phone is not exist";
                return View();
            }
        }
    }
}
