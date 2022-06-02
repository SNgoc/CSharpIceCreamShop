using IceCreamApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace IceCreamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        projectContext db = new projectContext();

        public FeedbackController(projectContext db)
        {
            this.db = db;
        }

        [HttpPost]
        public async Task<ActionResult> Create(Feedback fb)
        {
            if (fb == null)
            {
                return NotFound();
            }

            fb.CreateAt = DateTime.Now;
            db.Feedbacks.Add(fb);
            await db.SaveChangesAsync();
            return Ok(fb);
        }

        [HttpGet("All")]
        public async Task<ActionResult> GetAllFeedback()
        {
            var foundfb = await db.Feedbacks.ToListAsync();

            if (foundfb == null)
            {
                return NotFound();
            }

            return Ok(foundfb);
        }

        [HttpGet("Detail/{id}")]
        public async Task<ActionResult> Detail(int id)
        {
            var foundfb = await db.Feedbacks.FindAsync(id);

            if (foundfb == null)
            {
                return NotFound();
            }

            return Ok(foundfb);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var foundfb = await db.Feedbacks.FindAsync(id);

            if (foundfb == null)
            {
                return NotFound();
            }
            db.Feedbacks.Remove(foundfb);
            await db.SaveChangesAsync();
            return Ok();
        }


    }
}
