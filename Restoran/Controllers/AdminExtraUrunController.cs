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
    public class AdminExtraUrunController : Controller
    {
        // GET: AdminExtraUrun
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        public ActionResult Index()
        {
            var data = db.ExtraUruns.ToList();
            return View(data);
        }
        public ActionResult Create()
        {
            ViewBag.urunID = new SelectList(db.Uruns, "urunID", "urunAd");
            return View();
        }
        [HttpPost]
        public ActionResult Create(ExtraUrun extraurun)
        {
            if (ModelState.IsValid)
            {
                    db.ExtraUruns.Add(extraurun);
                    db.SaveChanges();
                    return RedirectToAction("Index", "AdminExtraUrun");
                        
            }
            ViewBag.urunID = new SelectList(db.Uruns, "urunID", "urunAd");
            return View(extraurun);
        }

        public ActionResult Edit(int extraUrunID)
        {
            var extralar = db.ExtraUruns.Where(x => x.extraUrunID == extraUrunID).SingleOrDefault();
            ViewBag.urunID = new SelectList(db.Uruns, "urunID", "urunAd", extralar.urunID);

            if (extralar == null)
            {
                return HttpNotFound();
            }
            return View(extralar);
        }
        [HttpPost]
        public ActionResult Edit(ExtraUrun extra, int extraUrunID)
        {
            if (ModelState.IsValid)
            {
                var extras = db.ExtraUruns.Where(u => u.extraUrunID == extraUrunID).SingleOrDefault();

                extras.extraUrunAdi = extra.extraUrunAdi;
                extras.extraUrunFiyat = extra.extraUrunFiyat;
                extras.extraUrunAdet = extra.extraUrunAdet;
                extras.active = extra.active;

                extras.urunID = extra.urunID;



                db.SaveChanges();
                return RedirectToAction("Index", "AdminExtraUrun", new { extraUrunID = extras.extraUrunID });
            }
            ViewBag.urunID = new SelectList(db.Uruns, "urunID", "urunAd", extra.urunID);

            return View(extra);

        }
        public JsonResult DeleteExtraUrunRecord(int extraUrunID)
        {
            bool result = false;
            ExtraUrun eua = db.ExtraUruns.Find(extraUrunID);
            if (eua != null)
            {
                eua.active = false;
                db.ExtraUruns.Remove(eua);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}