using ibernsof_Restoran.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;


namespace ibernsof_Restoran.Controllers
{
    public class UserController : Controller
    {
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        public ActionResult Index()
        {
            var oteller = db.Users.OrderByDescending(q => q.active).ToList();
            return View(oteller);
        }
        public ActionResult Create()
        {
            ViewBag.yetkiID = new SelectList(db.Yetkis, "yetkiID", "yetkiAd");
            return View();
        }
        [HttpPost]
        public ActionResult Create(User usr)
        {
            if (ModelState.IsValid)
            {

                usr.active = true;
                db.Users.Add(usr);
                db.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            ViewBag.yetkiID = new SelectList(db.Yetkis, "yetkiID", "yetkiAd");
            return View(usr);
        }
        public ActionResult Edit(int userID)
        {
            var userlar = db.Users.Where(x => x.userID == userID).SingleOrDefault();
            ViewBag.yetkiID = new SelectList(db.Yetkis, "yetkiID", "yetkiAd", userlar.yetkiID);
            if (userlar == null)
            {
                return HttpNotFound();
            }
            return View(userlar);
        }
        [HttpPost]
        public ActionResult Edit(User user, int userID)
        {
            if (ModelState.IsValid)
            {
                var users = db.Users.Where(u => u.userID == userID).SingleOrDefault();
               
                users.username = user.username;
                users.password = user.password;
                users.yetkiID = user.yetkiID;
                users.userDurum = user.userDurum;

                db.SaveChanges();
                return RedirectToAction("Index", "User", new { userID = users.userID });
            }
            ViewBag.yetkiID = new SelectList(db.Yetkis, "yetkiID", "yetkiAd", user.yetkiID);
            return View();
        }

        public JsonResult DeleteUserRecord(int userID)
        {
            bool result = false;
            User usr = db.Users.Find( userID);
            if (usr != null)
            {
                usr.active = false;
                db.Users.Remove(usr);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}