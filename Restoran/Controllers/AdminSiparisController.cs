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
    public class AdminSiparisController : Controller
    {
        // GET: Siparis
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();

        public ActionResult Index()
        {
          
            IEnumerable<CartItem> veri = null;

            veri = db.CartItems.GroupBy(cust => cust.cartNo ).Select(m => m.FirstOrDefault());

           
            var cartlar = veri.Where(x=>x.onay==false && x.siparisonay==true).OrderByDescending(m=>m.cartItemID).ToList();
            
         

            return View(cartlar);
        }
        public ActionResult Tour()
        {

          



            return View();
        }
        public ActionResult TumSiparis()
        {
            IEnumerable<CartItem> veri = null;

            veri = db.CartItems.GroupBy(cust => cust.cartNo).Select(m => m.FirstOrDefault());

            
            var cartlar = veri.Where(x => x.onay == true && x.active==true).OrderByDescending(m => m.cartItemID).ToList();
            return View(cartlar);
        }
        public ActionResult MasaSiparis()
        {
            IEnumerable<CartItem> veri = null;

            veri = db.CartItems.GroupBy(cust => cust.masaID).Select(m => m.FirstOrDefault());


            var cartlar = veri.Where(x => x.onay == false).OrderByDescending(m => m.cartItemID).ToList();



            return View(cartlar);
        }
        public ActionResult TumSiparisDetay(int cartNo)
        {

            CartItem model = new CartItem();


            model.cartlar = db.CartItems.Where(x => x.cartNo == cartNo).GroupBy(cust => cust.cartNo).Select(m => m.FirstOrDefault());

            model.cartt = db.CartItems.Where(x => x.cartNo == cartNo).ToList();


            return View(model);

        }
        public ActionResult MasaDetay(int masaID)
        {
            var listele = db.CartItems.Where(x => x.masaID==masaID).ToList();


            return View(listele);

        }
        public JsonResult OdenmediOnayRecord(int cartNo)
        {
            bool result = false;
            CartItem usr = db.CartItems.Where(x => x.cartNo == cartNo).FirstOrDefault(); ;
            if (usr != null)
            {
                usr.onay = false;
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SiparisDetay(int cartNo)
        {
            CartItem model = new CartItem();

          
            model.cartlar = db.CartItems.Where(x => x.cartNo == cartNo).GroupBy(cust => cust.cartNo).Select(m => m.FirstOrDefault());

            model.cartt = db.CartItems.Where(x => x.cartNo == cartNo ).ToList();
            ViewBag.sizeID = new SelectList(db.SizeProducts, "sizeID", "sizeName");

            return View(model);
        }
        public JsonResult DeleteSiparisRecord(int cartNo)
        {

            List<CartItem> crt = db.CartItems.Where(x => x.cartNo == cartNo).ToList();


            bool result = false;
          
            if (crt != null)
            {

                foreach (var item3 in crt)
                {
                    item3.active = false;

                    db.SaveChanges();
                    result = true;
                }

              
                
            
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OdemeOnayRecord(int cartNo)
        {
            bool result = false;
            List<CartItem> usr = db.CartItems.Where(x => x.cartNo == cartNo).ToList();
            if (usr != null)
            {
                foreach (var item2 in usr)
                {
                    item2.onay = true;
                    db.SaveChanges();
                    result = true;
                }
               
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SiparisOnayRecord(int cartNo)
        {
            bool result = false;
            List<CartItem> usr = db.CartItems.Where(x => x.cartNo == cartNo).ToList();

            if (usr != null)
            {
                
                var list = db.CartItems.Where(x => x.cartNo == cartNo).Select(t => new { urunId = t.urunID, urunMiktar = t.miktar }).ToList();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var urunStokEski = db.Uruns.Where(x => x.urunID == item.urunId).Select(t => t.urunStok).FirstOrDefault();
                        var urunler = db.Uruns.Where(k => k.urunID == item.urunId).SingleOrDefault();
                        int urunStok = Convert.ToInt32(urunStokEski);
                        int stok = urunStok - item.urunMiktar;
                        urunler.urunStok = stok;
                     
                      

                        db.SaveChanges();
                    }

                }
                foreach (var item1 in usr)
                {
                    item1.siparisonay = true;
                    db.SaveChanges();
                }
              
                result = true;
               
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}