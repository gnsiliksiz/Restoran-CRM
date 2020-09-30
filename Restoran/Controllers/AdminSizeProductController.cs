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
    public class AdminSizeProductController : Controller
    {
        // GET: AdminSizeProduct
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        public ActionResult Index()
        {
            var data = db.SizeProducts.ToList();
            return View(data);
        }
        public ActionResult Create()
        {
            ViewBag.urunID = new SelectList(db.Uruns, "urunID", "urunAd");
            return View();
        }
        [HttpPost]
        public ActionResult Create(SizeProduct extraurun)
        {
            if (ModelState.IsValid)
            {
                db.SizeProducts.Add(extraurun);
                db.SaveChanges();
                return RedirectToAction("Index", "AdminSizeProduct");

            }
            ViewBag.urunID = new SelectList(db.Uruns, "urunID", "urunAd");
            return View(extraurun);
        }

        public ActionResult Edit(int sizeID)
        {
            var extralar = db.SizeProducts.Where(x => x.sizeID == sizeID).SingleOrDefault();
            ViewBag.urunID = new SelectList(db.Uruns, "urunID", "urunAd", extralar.urunID);

            if (extralar == null)
            {
                return HttpNotFound();
            }
            return View(extralar);
        }
        [HttpPost]
        public ActionResult Edit(SizeProduct extra, int sizeID)
        {
            if (ModelState.IsValid)
            {
                var extras = db.SizeProducts.Where(u => u.sizeID == sizeID).SingleOrDefault();

                extras.sizeName = extra.sizeName;
                extras.sizeFiyat = extra.sizeFiyat;
                extras.urunID = extra.urunID;

                db.SaveChanges();
                return RedirectToAction("Index", "AdminSizeProduct", new { sizeID = extras.sizeID });
            }
            ViewBag.urunID = new SelectList(db.Uruns, "urunID", "urunAd", extra.urunID);

            return View(extra);

        }
        public JsonResult DeleteBoyutUrunRecord(int sizeID)
        {
            bool result = false;
            SizeProduct eua = db.SizeProducts.Find(sizeID);
            if (eua != null)
            {
               
                db.SizeProducts.Remove(eua);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}