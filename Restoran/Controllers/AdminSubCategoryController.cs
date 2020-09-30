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
    public class AdminSubCategoryController : Controller
    {
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        // GET: SubCategory
        public ActionResult Index()
        {
            var data = db.AltKategoris.Where(x => x.isDeleted == false).ToList();


            return View(data);
          }
        public ActionResult KategoriDetay( int id)
        {
            var data = db.Uruns.Where(x => x.altKatID == id).ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.kategoriID = new SelectList(db.Kategoris, "kategoriID", "KategoriAd");
            return View();
        }
        [HttpPost]
        public ActionResult Create(AltKategori altkategori, HttpPostedFileBase altKategoriResim)
        {
            int sess = Convert.ToInt32(Session["userID"]);
            if (ModelState.IsValid)
            {
                if (altKategoriResim != null)
                {
                    WebImage img = new WebImage(altKategoriResim.InputStream);
                    FileInfo fotoinfo = new FileInfo(altKategoriResim.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(925, 165);
                    img.Save("~/Uploads/KategoriPhoto/" + newfoto);
                    altkategori.altKatResim = "/Uploads/KategoriPhoto/" + newfoto;
                    altkategori.isDeleted = false;

                    db.AltKategoris.Add(altkategori);
                    db.SaveChanges();
                    return RedirectToAction("Index", "AdminSubCategory");
                }
                else
                {
                    ModelState.AddModelError("Fotoğraf", "Fotoğraf Seçiniz");
                }
            }
            return View(altkategori);
        }
        public ActionResult Edit(int altKatID)
        {
            var altkategoriler = db.AltKategoris.Where(x => x.altKatID == altKatID).SingleOrDefault();
            ViewBag.kategoriID = new SelectList(db.Kategoris, "kategoriID", "kategoriAd", altkategoriler.kategoriID);
            if (altkategoriler == null)
            {
                return HttpNotFound();
            }
            return View(altkategoriler);
        }
        [HttpPost]
        public ActionResult Edit(AltKategori altkategori, int altKatID, HttpPostedFileBase altKatResim)
        {
            if (ModelState.IsValid)
            {
                var altkategoris = db.AltKategoris.Where(k => k.altKatID == altKatID).SingleOrDefault();
                if (altKatResim != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(altkategori.altKatResim)))
                    {
                        System.IO.File.Delete(Server.MapPath(altkategori.altKatResim));
                    }
                    WebImage img = new WebImage(altKatResim.InputStream);
                    FileInfo fotoinfo = new FileInfo(altKatResim.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(925, 165);
                    img.Save("~/Uploads/" + newfoto);
                    altkategoris.altKatResim = "/Uploads/" + newfoto;
                }
                altkategoris.altKatAdi = altkategori.altKatAdi;
                altkategoris.altKatsira = altkategori.altKatsira;
                altkategoris.altKatDurum = altkategori.altKatDurum;
                altkategoris.kategoriID = altkategori.kategoriID;


                db.SaveChanges();
                return RedirectToAction("Index", "AdminSubCategory", new { altKatID = altkategoris.altKatID });
            }
            ViewBag.kategoriID = new SelectList(db.Kategoris, "kategoriID", "kategoriAd", altkategori.kategoriID);
            return View();
        }

        public JsonResult DeleteAltKategoriRecord(int altKatID)
        {
            bool result = false;
            AltKategori ktgr = db.AltKategoris.Find(altKatID);

            Urun urn = db.Uruns.Where(x => x.altKatID == altKatID).FirstOrDefault();
            if (ktgr != null)
            {
                if (urn == null) { 
                ktgr.isDeleted = true;
                db.AltKategoris.Remove(ktgr);
                db.SaveChanges();
                result = true;
                }else
                {
                    result = false;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}