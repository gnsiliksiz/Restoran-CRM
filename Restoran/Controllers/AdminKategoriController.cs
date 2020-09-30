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
    public class AdminKategoriController : Controller
    {
        // GET: AdminKategori
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        public ActionResult Index()
        {
            var data = db.Kategoris.Where(x => x.isDeleted == false).ToList();


            return View(data);
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Kategori kategori)
        {
            int sess = Convert.ToInt32(Session["userID"]);
            if (ModelState.IsValid)
            {
                   
                    kategori.isDeleted = false;
                  
                    db.Kategoris.Add(kategori);
                    db.SaveChanges();
                    return RedirectToAction("Index", "AdminKategori");
              
            }
            return View(kategori);
        }
        public ActionResult Edit(int kategoriID)
        {
            var kategoriler = db.Kategoris.Where(x => x.kategoriID == kategoriID).SingleOrDefault();
            ViewBag.kategoriID = new SelectList(db.Kategoris, "kategoriID", "kategoriAd", kategoriler.kategoriID);
            if (kategoriler == null)
            {
                return HttpNotFound();
            }
            return View(kategoriler);
        }
        [HttpPost]
        public ActionResult Edit(Kategori kategori, int kategoriID)
        {
            if (ModelState.IsValid)
            {
                var kategoris = db.Kategoris.Where(k => k.kategoriID == kategoriID).SingleOrDefault();
               
                kategoris.kategoriAd = kategori.kategoriAd;
              

                db.SaveChanges();
                return RedirectToAction("Index", "AdminKategori", new { kategoriID = kategoris.kategoriID });
            }

            return View();
        }

        public JsonResult DeleteKategoriRecord(int kategoriID)
        {
            bool result = false;
            Kategori ktgr = db.Kategoris.Find( kategoriID);
            AltKategori alt = db.AltKategoris.Where(x => x.kategoriID == kategoriID).FirstOrDefault();
            if (ktgr != null)
            {
                if (alt == null) { 
                ktgr.isDeleted = true;
                db.Kategoris.Remove(ktgr);
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