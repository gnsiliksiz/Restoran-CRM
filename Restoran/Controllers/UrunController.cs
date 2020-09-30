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
    public class UrunController : Controller
    {
       
            // GET: Urun
            ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
            public ActionResult Index()
            {
                int sess = Convert.ToInt32(Session["userID"]);
                var menuler = db.Uruns.Where(x => x.isDeleted == false && x.userID == sess).ToList();
               
                return View(menuler);
            }


            public ActionResult Create()
            {
            ViewBag.altKatID = new SelectList(db.AltKategoris, "altKatID", "altKatAdi");
            return View();
            }
            [HttpPost]
            public ActionResult Create(Urun urun, HttpPostedFileBase urunResim)
            {
                int sess = Convert.ToInt32(Session["userID"]);
                if (ModelState.IsValid)
                {
                    if (urunResim != null)
                    {
                        WebImage img = new WebImage(urunResim.InputStream);
                        FileInfo fotoinfo = new FileInfo(urunResim.FileName);

                        string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                        img.Resize(300, 200);
                        img.Save("~/Uploads/" + newfoto);
                        urun.urunResim = "/Uploads/" + newfoto;
                        urun.isDeleted = false;
                        urun.userID = sess;
                        db.Uruns.Add(urun);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Urun");
                    }
                    else
                    {
                        ModelState.AddModelError("Fotoğraf", "Fotoğraf Seçiniz");
                    }
                }
       
            return View(urun);
            }
            public ActionResult Edit(int urunID)
            {
                var menuler = db.Uruns.Where(x => x.urunID == urunID).SingleOrDefault();
                ViewBag.altKatID = new SelectList(db.AltKategoris, "altKatID", "altKatAdi", menuler.altKatID);
                if (menuler == null)
                {
                    return HttpNotFound();
                }
                return View(menuler);
            }
            [HttpPost]
            public ActionResult Edit(Urun urun, int urunID, HttpPostedFileBase urunResim)
            {
                if (ModelState.IsValid)
                {
                    var uruns = db.Uruns.Where(u => u.urunID == urunID).SingleOrDefault();
                    if (urunResim != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath(urun.urunResim)))
                        {
                            System.IO.File.Delete(Server.MapPath(uruns.urunResim));
                        }
                        WebImage img = new WebImage(urunResim.InputStream);
                        FileInfo fotoinfo = new FileInfo(urunResim.FileName);

                        string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                        img.Resize(300, 200);
                        img.Save("~/Uploads/" + newfoto);
                        uruns.urunResim = "/Uploads/" + newfoto;
                    }
                    uruns.urunAd = urun.urunAd;
                    uruns.urunAciklama = urun.urunAciklama;
                    uruns.urunFiyat = urun.urunFiyat;
                    uruns.altKatID = urun.altKatID;
                   uruns.urunStok = urun.urunStok;
                db.SaveChanges();
                    return RedirectToAction("Index", "Urun", new { urunID = uruns.urunID });
                }
            ViewBag.altKatID = new SelectList(db.AltKategoris, "altKatID", "altKatAdi", urun.altKatID);
            return View();
            }
            public JsonResult DeleteUrunRecord(int? urunID)
            {
                bool result = false;
                Urun urn = db.Uruns.Find(urunID);
            ExtraUrun ex = db.ExtraUruns.Where(x => x.urunID == urunID).FirstOrDefault();
            SizeProduct sz = db.SizeProducts.Where(x => x.urunID == urunID).FirstOrDefault();
                if (urn != null)
                {
                if(ex==null&& sz==null)
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