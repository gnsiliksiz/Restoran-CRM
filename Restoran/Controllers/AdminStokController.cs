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
    public class AdminStokController : Controller
    {
        // GET: AdminStok
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        public ActionResult Index()
        {
            
            int sess = Convert.ToInt32(Session["userID"]);
            var urunler = db.Uruns.Where(x => x.isDeleted == false && x.userID == sess).ToList();
          
            return View(urunler);
           
        }

       
        public ActionResult Edit(int urunID)
        {
            var menuler = db.Uruns.Where(x => x.urunID == urunID).SingleOrDefault();
       
            if (menuler == null)
            {
                return HttpNotFound();
            }
            return View(menuler);
        }
        [HttpPost]
        public ActionResult Edit(Urun urun, int urunID)
        {
            if (ModelState.IsValid)
            {
                var uruns = db.Uruns.Where(u => u.urunID == urunID).SingleOrDefault();
               
               
                uruns.urunStok = urun.urunStok;
                db.SaveChanges();
                return RedirectToAction("Index", "AdminStok", new { urunID = uruns.urunID });
            }
        
            return View();
        }
        public JsonResult DeleteStokRecord(int? urunID)
        {
            bool result = false;
            Urun urn = db.Uruns.Find(urunID);
            ExtraUrun ex = db.ExtraUruns.Where(x => x.urunID == urunID).FirstOrDefault();
            SizeProduct sz = db.SizeProducts.Where(x => x.urunID == urunID).FirstOrDefault();
            if (urn != null)
            {
                if (ex == null && sz == null)
                {
                    db.Uruns.Remove(urn);
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