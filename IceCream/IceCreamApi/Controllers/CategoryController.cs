using IceCreamApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        projectContext ctx;

        public CategoryController(projectContext ctx)
        {
            this.ctx = ctx;
        }

        //VIEW ALL Category
        [HttpGet] //fix lỗi swagger
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var result = await ctx.Categories.ToListAsync();
            if (result == null)
            {
                return NotFound();//mã 404
            }
            return Ok(result);//tìm thấy mã 200->299, mã 300 redirect
        }
        //END VIEW

        //CREATE CATEGORY
        //post create
        [HttpPost] //fix lỗi swagger
        public async Task<ActionResult<Category>> CreateCategory(Category cate)//đặt post ở đầu để đảm bảo post và fix lỗi swagger
        {
            ctx.Categories.Add(cate);
            await ctx.SaveChangesAsync();

            //tìm cate vừa tạo
            var ca = await ctx.Categories.OrderByDescending(c => c.CatId).SingleOrDefaultAsync();
            return CreatedAtAction("GetCategory", new { CatId = ca.CatId }, ca);
        }
        //END CREATE

        //update cate
        //lấy id theo CatId dùng cho hàm update
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var result = await ctx.Categories.SingleOrDefaultAsync(c => c.CatId == id);
            if (result == null)
            {
                return NotFound(); //mã 404
            }
            return Ok(result); //tìm thấy mã 200->299, mã 300 redirect
        }

        [HttpPut]//vì dùng method Put khác method update nên ko sợ trùng route
        public async Task<ActionResult> PutUpdateCategory(Category cate)//đặt put ở đầu để đảm bảo put
        {
            ctx.Entry(cate).State = EntityState.Modified;
            await ctx.SaveChangesAsync();

            return NoContent();
        }
        //end update
    }
}
