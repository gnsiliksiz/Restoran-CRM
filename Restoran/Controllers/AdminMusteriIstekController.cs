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
    public class AdminMusteriIstekController : Controller
    {
        // GET: AdminMusteriIstek
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        public ActionResult Index()
        {

            var data = db.MusteriIsteks.ToList();


            return View(data);
        
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
     
        public ActionResult Create(MusteriIstek mus, HttpPostedFileBase iconResim)
        {
          
            if (ModelState.IsValid)
            {
                if (iconResim != null)
                {
                    WebImage img = new WebImage(iconResim.InputStream);
                    FileInfo fotoinfo = new FileInfo(iconResim.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(16, 16);
                    img.Save("~/Uploads/" + newfoto);
                    mus.iconSinifAdi = "/Uploads/" + newfoto;
                
                 
                    db.MusteriIsteks.Add(mus);
                    db.SaveChanges();
                    return RedirectToAction("Index", "AdminMusteriIstek");
                }
                else
                {
                    ModelState.AddModelError("Fotoğraf", "Fotoğraf Seçiniz");
                }
            }

            return View(mus);
        }
        public ActionResult Edit(int musteriIstekID)
        {
            var mus = db.MusteriIsteks.Where(x => x.musteriIstekID == musteriIstekID).SingleOrDefault();
          
            if (mus == null)
            {
                return HttpNotFound();
            }
            return View(mus);
        }
        [HttpPost]
        public ActionResult Edit(MusteriIstek mus, int musteriIstekID, HttpPostedFileBase iconResim)
        {
            if (ModelState.IsValid)
            {
                var muss = db.MusteriIsteks.Where(u => u.musteriIstekID == musteriIstekID).SingleOrDefault();
                if (iconResim != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(mus.iconSinifAdi)))
                    {
                        System.IO.File.Delete(Server.MapPath(muss.iconSinifAdi));
                    }
                    WebImage img = new WebImage(iconResim.InputStream);
                    FileInfo fotoinfo = new FileInfo(iconResim.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(16, 16);
                    img.Save("~/Uploads/" + newfoto);
                    muss.iconSinifAdi = "/Uploads/" + newfoto;
                }
                muss.musteriIstekAdi= mus.musteriIstekAdi;
               
                db.SaveChanges();
                return RedirectToAction("Index", "AdminMusteriIstek", new { musteriIstekID = muss.musteriIstekID });
            }
         
            return View();
        }
        public JsonResult DeleteMusteriIstekRecord(int musteriIstekID)
        {
            bool result = false;
            MusteriIstek m = db.MusteriIsteks.Find(musteriIstekID);
            MusteriTalep alt = db.MusteriTaleps.Where(x => x.musteriIstekID == musteriIstekID).FirstOrDefault();
            if (m != null)
            {
                if (alt == null)
                {
                   
                    db.MusteriIsteks.Remove(m);
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}