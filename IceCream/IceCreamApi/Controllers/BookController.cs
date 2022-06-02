using IceCreamApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        projectContext ctx;
        

        public BookController(projectContext ctx)
        {
            this.ctx = ctx;
        }

        //VIEW ALL BOOK
        [HttpGet] //fix lỗi swagger
        public async Task<ActionResult<List<BookIceCream>>> GetBooks()
        {
            var result = await ctx.BookIceCreams.ToListAsync();
            if (result == null)
            {
                return NotFound();//mã 404
            }
            return Ok(result);//tìm thấy mã 200->299, mã 300 redirect
        }
        //END VIEW

        //VIEW BOOK BY CATEGORY
        [HttpGet("category/{CatId}")] //fix lỗi swagger
        public async Task<ActionResult<List<BookIceCream>>> GetBooksByCate(int CatId)
        {
            var result = await ctx.BookIceCreams.Where(c => c.CatId == CatId).ToListAsync();
            if (result == null)
            {
                return NotFound();//mã 404
            }
            return Ok(result);//tìm thấy mã 200->299, mã 300 redirect
        }
        //END VIEW

        //CREATE BOOK
        //post create
        [HttpPost] //fix lỗi swagger
        public async Task<ActionResult<BookIceCream>> CreateBook(BookIceCream book)//đặt post ở đầu để đảm bảo post và fix lỗi swagger
        {
            ctx.BookIceCreams.Add(book);
            await ctx.SaveChangesAsync();

            //tìm book vừa tạo
            var b = await ctx.BookIceCreams.OrderByDescending(c => c.BookId).SingleOrDefaultAsync();
            return CreatedAtAction("GetBooks", new { BookID = b.BookId }, b);
        }
        //END CREATE

        //update, details Book
        //lấy id theo bookId dùng cho hàm update
        [HttpGet("{id}")]
        public async Task<ActionResult<BookIceCream>> GetBook(int id)
        {
            var result = await ctx.BookIceCreams.SingleOrDefaultAsync(c => c.BookId == id);
            if (result == null)
            {
                return NotFound(); //mã 404
            }
            return Ok(result); //tìm thấy mã 200->299, mã 300 redirect
        }

        [HttpPut]//vì dùng method Put khác method update nên ko sợ trùng route
        public async Task<ActionResult> PutUpdateBook(BookIceCream book)//đặt put ở đầu để đảm bảo put
        {
            ctx.Entry(book).State = EntityState.Modified;
            await ctx.SaveChangesAsync();

            return NoContent();
        }
        //end update

        //update active status
        [HttpPost("status/{bookId}/{status}")]// dùng post thì bên kia phải là PostAsync
        public async Task<bool> GetStatusBook(int bookId, bool status)
        {
            var result = await ctx.BookIceCreams.SingleOrDefaultAsync(c => c.BookId == bookId);
            if (result == null)
            {
                return false; //mã 404
            }
            result.Active = status;
            try
            {
                return (await ctx.SaveChangesAsync() > 0);//có record
            }
            catch
            {

                return false;
            }
        }

        //Related Book
        [HttpGet("relatedBook/{number}")]
        public async Task<ActionResult<List<BookIceCream>>> findRelatedBook(int number)
        {
            var book = await ctx.BookIceCreams.OrderByDescending(b => b.BookId).Take(number).ToListAsync();
            if (book != null)
            {
                return Ok(book);
            }
            return NotFound();
        }

        //FindbyTitle filter by title
        //route: /api/book/searchTitle/{title}
        [HttpGet("searchTitle/{title}")] //fix lỗi swagger
        public async Task<ActionResult<List<BookIceCream>>> SearchByTitle(string title)
        {
            var result = await ctx.BookIceCreams.Where(t => t.Title.Contains(title)).Select(tt => new BookIceCream { BookId = tt.BookId, CatId = tt.CatId, Title = tt.Title, Author = tt.Author, Description = tt.Description, Price = tt.Price, Image = tt.Image, Active = tt.Active, CreateAt = tt.CreateAt }).ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
