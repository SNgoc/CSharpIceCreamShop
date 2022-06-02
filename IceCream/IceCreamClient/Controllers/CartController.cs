using IceCreamClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IceCreamClient.Controllers
{
    public class CartController : Controller
    {
        const string API_URl = "http://localhost/IceCreamApi";
        IHttpClientFactory _factory;

        public CartController(IHttpClientFactory factory)
        {
            _factory = factory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(int id, int quantity, string button)
        {
            HttpClient client = _factory.CreateClient();
            var result = await client.GetAsync(API_URl + $"/api/Book/{id}");
            var data = await result.Content.ReadAsStringAsync();
            var book = JsonConvert.DeserializeObject<BookIceCream>(data);
            client.Dispose();

            string json = HttpContext.Session.GetString("cart");
            List<Cart> listCart = new List<Cart>();

            if (json != null)
            {
                JArray jsonResponse = JArray.Parse(json);
                foreach (var item in jsonResponse)
                {
                    JObject cartResult = (JObject)item;
                    listCart.Add(JsonConvert.DeserializeObject<Cart>(cartResult.ToString()));
                }
            }

            if (button == "delete")
            {
                int delete_index = -1;
                foreach (Cart item in listCart)
                {
                    delete_index++;
                    if (item.BookIceCream.BookId == id)
                    {
                        break;
                    }
                }

                listCart.RemoveAt(delete_index);
            }
            else
            {
                Cart cart = listCart.Where(i => i.BookIceCream.BookId.Equals(id)).FirstOrDefault();

                if (listCart.Count > 0 && cart != null)
                {
                    cart.Quantity = quantity;
                }
                else
                {
                    Cart newProduct = new Cart { BookIceCream = book, Quantity = quantity, Price = book.Price };
                    listCart.Add(newProduct);
                }
            }

            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(listCart));
            return Redirect(Request.Headers["Referer"].ToString());
            //return RedirectToAction("Index", "Home");
        }
       
    }
}
