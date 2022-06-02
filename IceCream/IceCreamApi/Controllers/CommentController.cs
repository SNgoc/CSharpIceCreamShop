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
    public class CommentController : ControllerBase
    {
        projectContext _context;

        public CommentController(projectContext context)
        {
            _context = context;
        }

        [HttpGet("/api/comment")]
        public async Task<ActionResult<List<Comment>>> findAll()
        {
            var comments = await _context.Comments.ToListAsync();
            if (comments != null)
            {
                return Ok(comments);
            }
            return NotFound();
        }

        [HttpGet("/api/comment/{id}")]
        public async Task<ActionResult<Comment>> findByID(int id)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(c => c.Id == id);
            if (comment != null)
            {
                return Ok(comment);
            }
            return NotFound();
        }

        [HttpGet("/api/comment/byRecipe/{RecipeId}")]
        public async Task<ActionResult<Comment>> findByRecipeID(int RecipeId)
        {
            var comments = await _context.Comments.Where(c => c.RecipeId == RecipeId).ToListAsync();
            if (comments != null)
            {
                return Ok(comments);
            }
            return NotFound();
        }

        [HttpPost("/api/comment")]
        public async Task<ActionResult> postComment(Comment comment)
        {
            _context.Comments.Add(new Comment
            {
                Content = comment.Content,
                NameUser = comment.NameUser,
                EmailUser = comment.EmailUser,
                RecipeId = comment.RecipeId,
                Reply = comment.Reply,
                isReplied = Convert.ToBoolean(comment.isReplied) ? true : false,
                CreateAt = DateTime.Now
            });

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("/api/comment/{id}")]
        public async Task<ActionResult> replyComment(int id, Comment cmt)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(c => c.Id == id);
            comment.Reply = cmt.Reply;
            comment.isReplied = true;

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("/api/comment/{id}")]
        public async Task<ActionResult> deleteComment(int id)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(c => c.Id == id);
            _context.Comments.Remove(comment);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
