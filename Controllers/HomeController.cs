using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public ViewResult Index()
        {
            return View("Index");
        }

        [HttpPost("users/register")]
        public IActionResult Register(LogRegWrapper FromForm)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == FromForm.Register.Email))
                {
                    ModelState.AddModelError("Register.Email", "Already registered!");
                    return Index();
                }
            
            PasswordHasher<User>Hasher = new PasswordHasher<User>();
            FromForm.Register.Password = Hasher.HashPassword(FromForm.Register, FromForm.Register.Password);

            dbContext.Add(FromForm.Register);
            dbContext.SaveChanges();

            HttpContext.Session.SetInt32("UserId", FromForm.Register.UserId);
            return RedirectToAction("dashboard");
            }
            else
            {
                return Index();
            }
        }
        

        [HttpPost("login")]
        public IActionResult Login(LogRegWrapper FromForm)
        {
            if(ModelState.IsValid)
            {
                User InDb = dbContext.Users.FirstOrDefault(u => u.Email == FromForm.Login.LogEmail);
                if(InDb == null)
                {
                    ModelState.AddModelError("Login.LogEmail", "Invalid Email or Password");
                    return Index();
                }
                HttpContext.Session.SetInt32("UserId", InDb.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return Index();
            }
        }

        [HttpGet("logout")]
        public RedirectToActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Index");
            }
            DashWrapper Mod = new DashWrapper()
            {
                AllWeddings = dbContext.Weddings
                .Include(w => w.Planner)
                .Include(w => w.GuestsAttending)
                    .ThenInclude(r => r.Guest)
                    .Where(w => w.Date > DateTime.Today)
                    .ToList(),
                LoggedUser = dbContext.Users
                    .FirstOrDefault(u => u.UserId == (int)LoggedId)
            };

            return View("dashboard", Mod);
        }

        [HttpGet("weddings/new")]
        public IActionResult NewWedding()
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Index");
            }
            return View ("NewWedding");
        }

        [HttpPost("weddings/create")]
        public IActionResult CreateWedding(Wedding FromForm)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId ==  null)
            {
                return RedirectToAction("Index");
            }
            FromForm.UserId = (int)LoggedId;
            if(ModelState.IsValid)
            {
                if(FromForm.Date < DateTime.Today)
                {
                    ModelState.AddModelError("Date", "No time taveling, now!");
                    return NewWedding();
                }
                
                dbContext.Add(FromForm);
                dbContext.SaveChanges();
                return RedirectToAction("dashboard");
            }
            else{
                return NewWedding();
            }
        }
        

        [HttpGet("weddings/{WeddingId}")]
        public IActionResult WeddingEvent(int WeddingId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Index");
            }
            Wedding ToPage = dbContext.Weddings
                .Include(w => w.GuestsAttending)
                .ThenInclude(r => r.Guest)
                .FirstOrDefault(w => w.WeddingId == WeddingId);
            if(ToPage == null)
            {
                return RedirectToAction("dashboard");
            }
            return View("eventpage", ToPage);
        }
        
        
        [HttpGet("weddings/{WeddingId}/edit")]
        public IActionResult EditWedding(int WeddingId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("LogReg");
            }

            Wedding ToEdit = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == WeddingId);

            if(ToEdit == null || ToEdit.UserId != (int)LoggedId)
            {
                return RedirectToAction("dashboard");
            }

            return View("EditWedding", ToEdit);
        }
        
        [HttpPost("weddings/{WeddingId}/update")]
        public IActionResult UpdateWedding (int WeddingId, Wedding FromForm)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Index");
            }
        
            if(!dbContext.Weddings.Any(w => w.WeddingId == WeddingId && w.UserId == (int)LoggedId))
            {
                return RedirectToAction("dashboard");
            }
            FromForm.UserId = (int)LoggedId;
            if(ModelState.IsValid)
            {
                FromForm.WeddingId = WeddingId;
                dbContext.Update(FromForm);
                dbContext.Entry(FromForm).Property("CreatedAt").IsModified = false;
                dbContext.SaveChanges();
                return RedirectToAction("dashboard");
            }
            else
            {
                return EditWedding(WeddingId);
            }
        }

        [HttpGet("weddings/{WeddingId}/rsvp")]
        public RedirectToActionResult RSVP(int WeddingId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Index");
            }
            Wedding ToJoin = dbContext.Weddings
                .Include(w => w.GuestsAttending)
                .FirstOrDefault(w => w.WeddingId == WeddingId);

            if(ToJoin == null || ToJoin.UserId == (int)LoggedId || ToJoin.GuestsAttending.Any(r => r.UserId == (int)LoggedId))
            {
                return RedirectToAction("dashboard");
            }
            else
            {
                RSVP NewRsvp = new RSVP()
                {
                    UserId = (int)LoggedId,
                    WeddingId = WeddingId
                };
                dbContext.Add(NewRsvp);
                dbContext.SaveChanges();
                return RedirectToAction("dashboard");
            }
        }

        [HttpGet("weddings/{WeddingId}/unrsvp")]
        public RedirectToActionResult UnRSVP(int WeddingId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Index");
            }
            Wedding ToLeave = dbContext.Weddings
                .Include(w => w.GuestsAttending)
                .FirstOrDefault(w => w.WeddingId == WeddingId);

            if(ToLeave == null || !ToLeave.GuestsAttending.Any(r => r.UserId == (int)LoggedId))
            {
                return RedirectToAction("dashboard");
            }
            else
            {
                RSVP ToRemove = dbContext.RSVPs.FirstOrDefault(r => r.UserId == (int)LoggedId && r.WeddingId == WeddingId);
                dbContext.Remove(ToRemove);
                dbContext.SaveChanges();

                return RedirectToAction("dashboard");
            }
        }

        [HttpGet("weddings/{WeddingId}/delete")]
        public RedirectToActionResult DeleteWedding(int WeddingId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Index");
            }

            Wedding ToDelete = dbContext.Weddings
                .FirstOrDefault(w => w.WeddingId == WeddingId);

            if(ToDelete == null || ToDelete.UserId != (int)LoggedId)
            {
                return RedirectToAction("dashboard");
            }

            dbContext.Remove(ToDelete);
            dbContext.SaveChanges();
            return RedirectToAction("dashboard");
        }


    }
}