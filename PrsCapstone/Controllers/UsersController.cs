using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsCapstone.Models;

namespace PrsCapstone.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly PrsCapstoneContext _context;

        public UsersController(PrsCapstoneContext context) {
            _context = context;
        }

        [HttpGet("login/{username}/{password}")]
        public async Task<ActionResult<User>> Login(string username, string password) {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        [HttpGet("recoverpassword_e/{username}/{email}")]
        public async Task<bool> RecoverPasswordWithEmail(string username, string email) {
            if (await _context.Users.SingleOrDefaultAsync(u => u.Username == username && u.Email == email) == null) {
                return false;
            }
            return true;
        }
        [HttpGet("recoverpassword_p/{username}/{phone}")]
        public async Task<bool> RecoverPasswordWithPhone(string username, string phone) {
            if (await _context.Users.SingleOrDefaultAsync(u => u.Username == username && u.Phone == phone) == null) {
                return false;
            }
            return true;
        }

        [HttpGet("resetpassword/{password}")]
        public async Task<IActionResult> ResetPassword(string password, User user) {
            user.Password = password;
            return await PutUser(user.Id, user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id) {
            var user = await _context.Users.FindAsync(id);

            if (user == null) {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user) {
            if (id != user.Id) {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!UserExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user) {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id) {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id) {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
