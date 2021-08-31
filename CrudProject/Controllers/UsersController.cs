using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudProject.Data;
using CrudProject.Models;
using Microsoft.AspNetCore.Http;

namespace CrudProject.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
      
        private readonly UserContext _context;
        private readonly IUserRepo<User> _repo;

        public UsersController(UserContext context, IUserRepo<User> repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        [Route("/allUsers")]
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.User.OrderByDescending(p => p.Id);
        }

        [Route("/login")]
        [HttpGet]
        public IActionResult Login(string Name,string Password)
        {
            var check =  _context.User.Where(x => x.Name == Name && x.Password == Password);
            if(check != null)
            {
                HttpContext.Session.SetString("username", Name);
                return View("Success");
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("Index");
            }
        }

        [Route("/logOut")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index");
        }

        [Route("/allUsersById")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blogPost = await _context.User.FindAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(blogPost);
        }

        [Route("/editUser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User usr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usr.Id)
            {
                return BadRequest();
            }

            _context.Entry(usr).State = EntityState.Modified;

            try
            {
                _repo.Update(usr);
                var save = await _repo.SaveAsync(usr);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool Exists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        [Route("/createUser")]
        [HttpPost]
        public async Task<IActionResult> UserPost([FromBody] User usr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.Add(usr);
            var save = await _repo.SaveAsync(usr);

            return CreatedAtAction("GetUser", new { id = usr.Id }, usr);
        }

        [Route("/deleteUser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _repo.Delete(user);
            var save = await _repo.SaveAsync(user);

            return Ok(user);
        }

    }
}
