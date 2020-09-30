using ibernsof_Restoran.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Restoran.Controllers
{
    public class AdminMasaKategoriController : Controller
    {
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        public ActionResult Index()
        {
            var data = db.masaKategoris.Where(x => x.isactive == false).ToList();


            return View(data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(masaKategori masakategori)
        {
            int sess = Convert.ToInt32(Session["userID"]);
            if (ModelState.IsValid)
            {

                masakategori.isactive = false;

                db.masaKategoris.Add(masakategori);
                db.SaveChanges();
                return RedirectToAction("Index", "AdminMasaKategori");

            }
            return View(masakategori);
        }
        public ActionResult Edit(int masaKatID)
        {
            var kategoriler = db.masaKategoris.Where(x => x.masaKatID == masaKatID).SingleOrDefault();
            ViewBag.masaKatID = new SelectList(db.Kategoris, "masaKatID", "masaKatAdi", kategoriler.masaKatID);
            if (kategoriler == null)
            {
                return HttpNotFound();
            }
            return View(kategoriler);
        }
        [HttpPost]
        public ActionResult Edit(masaKategori masa, int masaKatID)
        {
            if (ModelState.IsValid)
            {
                var masas = db.masaKategoris.Where(k => k.masaKatID == masaKatID).SingleOrDefault();

                masas.masaKatAdi = masa.masaKatAdi;


                db.SaveChanges();
                return RedirectToAction("Index", "AdminMasaKategori", new { masaKatID = masas.masaKatID });
            }

            return View();
        }

        public JsonResult DeleteMasaKategoriRecord(int masaKatID)
        {
            bool result = false;
            masaKategori ktgr = db.masaKategoris.Find(masaKatID);
            Masa alt = db.Masas.Where(x => x.masaKatID == masaKatID).FirstOrDefault();
            if (ktgr != null)
            {
                if (alt == null)
                {
                    ktgr.isactive = true;
                    db.masaKategoris.Remove(ktgr);
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