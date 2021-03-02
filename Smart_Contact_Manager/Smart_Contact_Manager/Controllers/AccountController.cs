using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SmartContactManager.Models;
using Smart_Contact_Manager.Models;
using Smart_Contact_Manager.Models.ViewModels;

namespace Smart_Contact_Manager.Controllers
{

    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AppDbContext db = AppDbContext.GetDbContext();

        // GET: api/Account
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }


        // GET: api/Account/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        // PUT: api/Account/5
        [AcceptVerbs("PUT")]
        [HttpPut]
        public async Task<IHttpActionResult> PutUser(int id, RegisterUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            User updatedUser = new User()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber
            };
            db.Entry(updatedUser).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(new { status = 200, isSuccess = true, message = "User details modified" });
        }


        // POST: api/Account/Register
        [AcceptVerbs("POST")]
        [Route("Register")]
        [HttpPost]
        public IHttpActionResult Register(RegisterUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(UserExistsWithUsername(user.Email))
            {
                string message = "User already exist";
                ModelState.AddModelError("Error", message);
                return BadRequest(ModelState);
            }

            User newUser = new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
            };
            db.Users.Add(newUser);
            db.SaveChanges();

            return Ok(newUser);
        }

        // POST: api/Account
        [AcceptVerbs("POST")]
        [Route("Login")]
        [HttpPost]
        public IHttpActionResult Login(LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(db.Users.Count(u => u.Email == user.Email && u.Password == user.Password) == 0)
            {
                /*string message = "Invalid Credentials";
                ModelState.AddModelError("Error", message);
                return BadRequest(ModelState);*/
                return Ok(new { status = 401, isSuccess = false, message = "Invalid User Credentials" });
            }

            User loggedUser = FindUserByUsername(user.Email);
            return Ok(new { status = 200, isSuccess = true, message = "User Login successfully", UserDetails = loggedUser });
        }


        // PUT: api/Account/ResetPassword
        [AcceptVerbs("PUT")]
        [Route("ResetPassword")]
        [HttpPut]
        public async Task<IHttpActionResult> ResetPassword(ResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = FindUserById(model.UserId);
            user.Password = model.Password;
            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(model.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(new { status = 200, isSuccess = true, message = "Password is successfully changed" });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }

        private User FindUserByUsername(string username)
        {
            return db.Users.Where(u => u.Email == username).FirstOrDefault();
        }

        private User FindUserById(int id)
        {
            return db.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        private bool UserExistsWithUsername(string username)
        {
            return db.Users.Count(e => e.Email == username) > 0;
        }
    }
}