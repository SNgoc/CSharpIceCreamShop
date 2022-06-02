using IceCreamClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamClient.Controllers
{
    public class BookController : Controller
    {
        const string BASE_URL = "http://localhost:47255";
        IHttpClientFactory factory;

        public BookController(IHttpClientFactory factory)
        {
            this.factory = factory;
        }

        //view list book
        public async Task<IActionResult> ShowBooks(string title, int? page)
        {
            HttpClient client = factory.CreateClient();//tạo và nhận data

            //show Category
            var responseCate = await client.GetStringAsync(BASE_URL + $"/api/category");
            var cate = JsonConvert.DeserializeObject<List<Category>>(responseCate);
            TempData["Categories"] = cate;
            //end

            //show relate book
            var responseRelate = await client.GetStringAsync(BASE_URL + $"/api/book/relatedBook/3");
            var relateBook = JsonConvert.DeserializeObject<List<BookIceCream>>(responseRelate);
            ViewData["RelateBook"] = relateBook;
            //end

            //PAGINATION
            const int LIMIT = 6;
            IPagedList<BookIceCream> onePageofItem;

            if (title == null)//pagination by view all, no search
            {
                //SHOWBOOKS
                var result = await client.GetStringAsync(BASE_URL + "/api/book");
                var books = JsonConvert.DeserializeObject<List<BookIceCream>>(result);
                ViewData["ListBook"] = books;
                //END show

                int totalItem = books.Count();
                int totalPage = 1;
                if (totalItem % LIMIT == 0)
                {
                    totalPage = totalItem / LIMIT;
                }
                else
                {
                    totalPage = totalItem / LIMIT + 1;
                }

                var pageNumber = page ?? 1;
                onePageofItem = books.ToPagedList(pageNumber, LIMIT);

                TempData["TotalPage"] = totalPage;
                TempData["CurrentPage"] = page;
                //END PAGINATION
            }
            else// pagination by result search
            {
                //SEARCH TITLE
                var result = await client.GetStringAsync(BASE_URL + $"/api/book/searchTitle/{title}");
                var books = JsonConvert.DeserializeObject<List<BookIceCream>>(result);

                int totalItem = books.Count();
                int totalPage = 1;
                if (totalItem % LIMIT == 0)
                {
                    totalPage = totalItem / LIMIT;
                }
                else
                {
                    totalPage = totalItem / LIMIT + 1;
                }

                var pageNumber = page ?? 1;
                onePageofItem = books.ToPagedList(pageNumber, LIMIT);

                TempData["Title"] = title;
                TempData["TotalPage"] = totalPage;
                TempData["CurrentPage"] = page;
                //END PAGINATION
            }

            client.Dispose();
            return View(onePageofItem);//trả về view obj book hiện tại để hiển thị thông tin book, ko có sẽ bị lỗi null model khi show list bên ShowBooks.cshtml => hiển thị view theo pagination
        }
        //end view book

        //view book by category
        public async Task<IActionResult> ShowBookByCategory(int CatId, int? page)
        {
            HttpClient client = factory.CreateClient();
            var result = await client.GetStringAsync(BASE_URL + $"/api/book/category/{CatId}");
            var books = JsonConvert.DeserializeObject<List<BookIceCream>>(result);

            //FIX ERROR NULL OBJ

            //show Category
            var responseCate = await client.GetStringAsync(BASE_URL + $"/api/category");
            var cate = JsonConvert.DeserializeObject<List<Category>>(responseCate);
            TempData["Categories"] = cate;
            //end

            //show relate book
            var responseRelate = await client.GetStringAsync(BASE_URL + $"/api/book/relatedBook/3");
            var relateBook = JsonConvert.DeserializeObject<List<BookIceCream>>(responseRelate);
            ViewData["RelateBook"] = relateBook;
            //end

            //END FIX

            //PAGINATION
            const int LIMIT = 6;
            IPagedList<BookIceCream> onePageofItem;

            int totalItem = books.Count();
            int totalPage = 1;
            if (totalItem%LIMIT == 0)
            {
                totalPage = totalItem / LIMIT;
            }
            else
            {
                totalPage = totalItem / LIMIT + 1;
            }

            var pageNumber = page ?? 1;
            onePageofItem = books.ToPagedList(pageNumber, LIMIT);

            TempData["CategoryID"] = CatId;
            TempData["TotalPage"] = totalPage;
            TempData["CurrentPage"] = page;
            //END

            client.Dispose();
            return View(onePageofItem);
        }
        //end view book

        //DETAILS BOOK
        public async Task<IActionResult> BookDetails(int bookId) // bookId phải giống với link bookId = item.BookId bên ShowBooks.cshtml
        {
            HttpClient client = factory.CreateClient();

            var response = await client.GetStringAsync(BASE_URL + $"/api/book/{bookId}");
            var book = JsonConvert.DeserializeObject<BookIceCream>(response);

            //show relate book
            var responseRelate = await client.GetStringAsync(BASE_URL + $"/api/book/relatedBook/4");
            var relateBook = JsonConvert.DeserializeObject<List<BookIceCream>>(responseRelate);
            ViewData["RelateBook"] = relateBook;

            client.Dispose();
            return View(book);
        }
    }
}
