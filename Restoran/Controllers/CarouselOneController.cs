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
    public class CarouselOneController : Controller
    {
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        // GET: CarouselOne
        public ActionResult Index()
        {
            var one = db.CarouselOnes.ToList();

            return View(one);
        }

        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Create(CarouselOne one, HttpPostedFileBase reklamResim)
        {
            int sess = Convert.ToInt32(Session["reklamID"]);
            if (ModelState.IsValid)
            {
                if (reklamResim != null)
                {
                    WebImage img = new WebImage(reklamResim.InputStream);
                    FileInfo fotoinfo = new FileInfo(reklamResim.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(531, 366);
                    img.Save("~/Uploads/" + newfoto);
                    one.reklamResim = "/Uploads/" + newfoto;
                    one.isDeleted = false;
                    one.reklamID = sess;
                    db.CarouselOnes.Add(one);
                    db.SaveChanges();
                    return RedirectToAction("Index", "CarouselOne");
                }
                else
                {
                    ModelState.AddModelError("Fotoğraf", "Fotoğraf Seçiniz");
                }
            }
            return View(one);
        }
        public ActionResult Edit(int reklamID)
        {
            var ones = db.CarouselOnes.Where(x => x.reklamID == reklamID).SingleOrDefault();
          
            if (ones == null)
            {
                return HttpNotFound();
            }
            return View(ones);
        }
        [HttpPost]
        public ActionResult Edit(CarouselOne one, int reklamID, HttpPostedFileBase reklamResim)
        {
            if (ModelState.IsValid)
            {
                var ones = db.CarouselOnes.Where(u => u.reklamID == reklamID).SingleOrDefault();
                if (reklamResim != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(one.reklamResim)))
                    {
                        System.IO.File.Delete(Server.MapPath(ones.reklamResim));
                    }
                    WebImage img = new WebImage(reklamResim.InputStream);
                    FileInfo fotoinfo = new FileInfo(reklamResim.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(530, 365);
                    img.Save("~/Uploads/" + newfoto);
                    ones.reklamResim = "/Uploads/" + newfoto;
                }
                ones.reklamHeader = one.reklamHeader;
                ones.reklamAciklamaHeader = one.reklamAciklamaHeader;
                ones.reklamAciklama = one.reklamAciklama;
                ones.reklamSira = one.reklamSira;
                db.SaveChanges();
                return RedirectToAction("Index", "CarouselOne", new { reklamID = ones.reklamID });
            }
           
            return View();
        }

        public JsonResult DeleteReklamRecord(int? reklamID)
        {
            bool result = false;
            CarouselOne one = db.CarouselOnes.Find( reklamID);
            if (one != null)
            {
                one.isDeleted = true;
                db.CarouselOnes.Remove(one);
                db.SaveChanges();
                    result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}