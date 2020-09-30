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
    public class CarouselTwoController : Controller
    {

        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        // GET: CarouselOne
        public ActionResult Index()
        {
           

            var two = db.CarouselTwoes.ToList();

            return View(two);
        }

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(CarouselTwo ttt, HttpPostedFileBase reklamtwoResim)
        {
            int sess = Convert.ToInt32(Session["reklamtwoID"]);
            if (ModelState.IsValid)
            {
                if (reklamtwoResim != null)
                {
                    WebImage img = new WebImage(reklamtwoResim.InputStream);
                    FileInfo fotoinfo = new FileInfo(reklamtwoResim.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(531, 366);
                    img.Save("~/Uploads/slider" + newfoto);
                    ttt.reklamtwoResim = "/Uploads/slider" + newfoto;
                    ttt.isDeleted = false;
                    ttt.reklamtwoID = sess;
                    db.CarouselTwoes.Add(ttt);
                    db.SaveChanges();
                    return RedirectToAction("Index", "CarouselTwo");
                }
                else
                {
                    ModelState.AddModelError("Fotoğraf", "Fotoğraf Seçiniz");
                }
            }
            return View(ttt);
        }
        public ActionResult Edit(int reklamtwoID)
        {
            var ones = db.CarouselTwoes.Where(x => x.reklamtwoID == reklamtwoID).SingleOrDefault();

            if (ones == null)
            {
                return HttpNotFound();
            }
            return View(ones);
        }
        [HttpPost]
        public ActionResult Edit(CarouselTwo one, int reklamtwoID, HttpPostedFileBase reklamtwoResim)
        {
            if (ModelState.IsValid)
            {
                var ones = db.CarouselTwoes.Where(u => u.reklamtwoID == reklamtwoID).SingleOrDefault();
                if (reklamtwoResim != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(one.reklamtwoResim)))
                    {
                        System.IO.File.Delete(Server.MapPath(ones.reklamtwoResim));
                    }
                    WebImage img = new WebImage(reklamtwoResim.InputStream);
                    FileInfo fotoinfo = new FileInfo(reklamtwoResim.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(531, 365);
                    img.Save("~/Uploads/" + newfoto);
                    ones.reklamtwoResim = "/Uploads/" + newfoto;
                }
                ones.reklamtwoHeader = one.reklamtwoHeader;
                ones.reklamtwoAciklamaHeader = one.reklamtwoAciklamaHeader;
                ones.reklamtwoAciklama = one.reklamtwoAciklama;
                ones.reklamtwoSira = one.reklamtwoSira;
                db.SaveChanges();
                return RedirectToAction("Index", "CarouselTwo", new { reklamtwoID = ones.reklamtwoID });
            }

            return View();
        }

        public JsonResult DeleteReklamRecord(int reklamtwoID)
        {
            bool result = false;
          
            CarouselTwo one = db.CarouselTwoes.Find( reklamtwoID);
            if (one != null)
            {
                one.isDeleted = true;
                db.CarouselTwoes.Remove(one);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}