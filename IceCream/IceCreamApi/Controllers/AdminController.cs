using IceCreamApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IceCreamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        projectContext db = new projectContext();

        public AdminController(projectContext db)
        {
            this.db = db;
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login(Admin admin)
        {
            var foundAdmin = await db.Admins.FirstOrDefaultAsync(u => u.Username == admin.Username && u.Password == admin.Password);

            if (foundAdmin == null)
            {
                return NotFound();
            }

            return Ok(foundAdmin);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Detail(int id)
        {
            var foundAdmin = await db.Admins.FindAsync(id);

            if (foundAdmin == null)
            {
                return NotFound();
            }

            return Ok(foundAdmin);
        }

        [HttpGet("Password/{id}/{oldpass}/{newpass}")]
        public async Task<ActionResult> ChangePassword(int id, string oldpass, string newpass)
        {


            var foundAdmin = await db.Admins.FindAsync(id);
            if (foundAdmin == null)
            {
                return NotFound();
            }

            if (!oldpass.Equals(foundAdmin.Password))
            {
                return NotFound("Old password is not correct.");
            }

            if (newpass.Equals(oldpass))
            {
                return NotFound("New password must be different old password.");
            }

            foundAdmin.Password = newpass;

            db.Admins.Update(foundAdmin);
            await db.SaveChangesAsync();

            return Ok(foundAdmin);
        }

        [HttpGet("All")]
        public async Task<ActionResult> ViewAll(int id)
        {
            var foundAdmins = await db.Admins.ToListAsync();

            if (foundAdmins == null)
            {
                return NotFound();
            }

            return Ok(foundAdmins);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddNew(Admin admin)
        {
            if (admin == null)
            {
                return NotFound();
            }

            var adminExisted = await db.Members.FirstOrDefaultAsync(u => u.Username == admin.Username);
            if (adminExisted != null)
            {
                return NotFound("Username is existed");
            }
            db.Admins.Add(admin);
            await db.SaveChangesAsync();
            return Ok(admin);
        }
    }
}
