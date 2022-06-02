using IceCreamApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        projectContext db = new projectContext();

        public MemberController(projectContext db)
        {
            this.db = db;
        }

        [HttpGet("Login/{username}/{password}")]
        public async Task<ActionResult> Login(string username, string password)
        {
            var foundMember = await db.Members.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (foundMember == null)
            {
                return NotFound();
            }

            if (foundMember.IsMember == false)
            {
                return NotFound();
            }

            double day = 0;
            if (foundMember.IdMemberOption == 1)
            {
                day = Math.Ceiling((Convert.ToDateTime(foundMember.RegisterDay).AddDays(+30) - DateTime.Now).TotalDays);
            }
            else
            {
                day = Math.Ceiling((Convert.ToDateTime(foundMember.RegisterDay).AddDays(+365) - DateTime.Now).TotalDays);
            }
            if (day < 0)
            {
                return NotFound();
            }
            return Ok(foundMember);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(Member member)
        {
            if (member == null)
            {
                return NotFound();
            }

            var userExisted = db.Members
               .FirstOrDefault(u => u.Username == member.Username);
            if (userExisted != null)
            {
                return NotFound("Username is existed");
            }

            member.RegisterDay = DateTime.Now;
            member.IsMember = true;
            db.Members.Add(member);
            await db.SaveChangesAsync();
            return Ok(member);
        }

        [HttpGet("Detail/{id}")]
        public async Task<ActionResult> Detail(int id)
        {
            var foundUser = await db.Members.FindAsync(id);

            if (foundUser == null)
            {
                return NotFound("User not found.");
            }

            return Ok(foundUser);
        }

        [HttpGet("All")]
        public async Task<ActionResult> GetAllUsers()
        {
            var foundUsers = await db.Members.ToListAsync();

            if (foundUsers == null)
            {
                return NotFound("User not found.");
            }

            return Ok(foundUsers);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(Member updateUser)
        {
            var foundUser = await db.Members.FindAsync(updateUser.Id);
            foundUser.Email = updateUser.Email;
            foundUser.Fullname = updateUser.Fullname;

            db.Members.Update(foundUser);
            await db.SaveChangesAsync();

            return Ok(updateUser);
        }

        [HttpGet("Password/{id}/{oldpass}/{newpass}")]
        public async Task<ActionResult> ChangePassword(int id, string oldpass, string newpass)
        {


            var foundUser = await db.Members.FindAsync(id);
            if (foundUser == null)
            {
                return NotFound("User not found.");
            }

            if (!oldpass.Equals(foundUser.Password))
            {
                return NotFound("Old password is not correct.");
            }

            if (newpass.Equals(oldpass))
            {
                return NotFound("New password must be different old password.");
            }

            foundUser.Password = newpass;

            db.Members.Update(foundUser);
            await db.SaveChangesAsync();

            return Ok(foundUser);
        }

        [HttpGet("Deactive/{id}")]
        public async Task<ActionResult> Deactive(int id)
        {
            var foundUser = await db.Members.SingleOrDefaultAsync(m => m.Id == id);
            if (foundUser == null)
            {
                return NotFound("User not found.");
            }

            foundUser.IsMember = false;

            db.Members.Update(foundUser);
            await db.SaveChangesAsync();
            return Ok(foundUser);
        }

        [HttpGet("Active/{id}")]
        public async Task<ActionResult> Active(int id)
        {
            var foundUser = await db.Members.SingleOrDefaultAsync(m => m.Id == id);
            if (foundUser == null)
            {
                return NotFound("User not found.");
            }

            foundUser.IsMember = true;

            db.Members.Update(foundUser);
            await db.SaveChangesAsync();
            return Ok(foundUser);
        }

        [HttpGet("ResetPassword/{id}")]
        public async Task<ActionResult> ResetPassword(int id)
        {
            var foundUser = await db.Members.SingleOrDefaultAsync(m => m.Id == id);
            if (foundUser == null)
            {
                return NotFound("User not found.");
            }

            foundUser.Password = "123456";

            db.Members.Update(foundUser);
            await db.SaveChangesAsync();
            return Ok(foundUser);
        }
    }
}
