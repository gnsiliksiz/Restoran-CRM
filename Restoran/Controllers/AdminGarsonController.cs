 using ibernsof_Restoran.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Configuration;
using Newtonsoft.Json;

namespace ibernsof_Restoran.Controllers
{
    public class AdminGarsonController : Controller
    {
        // GET: AdminGarson
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();

 
        public ActionResult Index(int? masaKatID)
        {

            Session["masaKatAdi"] = "Bahce";
            
            if (masaKatID != null)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["masaKatID"]))
                {
                    // Query string value is there so now use it
                    int masaKat = Convert.ToInt32(Request.QueryString["masaKatID"]);

                    Session["masaKatID"] = masaKat;
                    var masaKatAdi = db.masaKategoris.Where(x => x.masaKatID == masaKatID).Select(x => x.masaKatAdi).FirstOrDefault();
                    Session["masaKatAdi"] = masaKatAdi.ToString();

                }
                var result2 =
                from p1 in db.Masas
                join c1 in db.CartItems on p1.masaID equals c1.masaID into j1
                from j2 in j1.DefaultIfEmpty()
                where p1.masaKatID == masaKatID
                group j2 by new { p1.masaID, p1.masaNo, p1.masaKategori.masaKatAdi, p1.masaKatID } into grouped
                select new { masaID = grouped.Key.masaID, masaNo = grouped.Key.masaNo, Count = grouped.Where(r => r.siparisonay == false).Select(c => c.cartNo).Distinct().Count(), siparisOnay = grouped.Where(o => o.siparisonay == false || o.siparisonay == null).Select(t1 => t1.siparisonay).FirstOrDefault(), masaKatID = grouped.Key.masaKatID, masaKatAdi = grouped.Key.masaKatAdi };

                string b = JsonConvert.SerializeObject(result2);
                List<masaCount> listMasa = new List<masaCount>();

                foreach (var item in result2)
                {
                    listMasa.Add(new masaCount { masaID = item.masaID, siparisCount = item.Count, siparisOnay = item.siparisOnay, masaNo = item.masaNo, masaKatID = item.masaKatID, masaKatAdi = item.masaKatAdi });

                }
                ViewBag.result = listMasa;
            }

            /// sayfa açıldığında ilk masa katının id ve adini almak için ekledim
            var ilkMasaKatId = db.masaKategoris.Where(x => x.isactive == false).Select(m => m.masaKatID).FirstOrDefault();
         

            if (masaKatID == null)
            {
                Session["masaKatID"] = Convert.ToInt32(ilkMasaKatId);
                var result2 =
                  from p1 in db.Masas
                  join c1 in db.CartItems on p1.masaID equals c1.masaID into j1
                  from j2 in j1.DefaultIfEmpty()
                  where p1.masaKatID == ilkMasaKatId
                  group j2 by new { p1.masaID, p1.masaNo, p1.masaKategori.masaKatAdi, p1.masaKatID } into grouped
                  select new { masaID = grouped.Key.masaID, masaNo = grouped.Key.masaNo, Count = grouped.Where(r => r.siparisonay == false).Select(c => c.cartNo).Distinct().Count(), siparisOnay = grouped.Where(o => o.siparisonay == false || o.siparisonay == null).Select(t1 => t1.siparisonay).FirstOrDefault(), masaKatID = grouped.Key.masaKatID, masaKatAdi = grouped.Key.masaKatAdi };

                string b = JsonConvert.SerializeObject(result2);
                List<masaCount> listMasa = new List<masaCount>();

                foreach (var item in result2)
                {
                    listMasa.Add(new masaCount { masaID = item.masaID, siparisCount = item.Count, siparisOnay = item.siparisOnay, masaNo = item.masaNo, masaKatID = item.masaKatID, masaKatAdi = item.masaKatAdi });

                }
                ViewBag.result = listMasa;

            }

            masaKategori modell = new masaKategori();

            //var masalar = db.Masas.ToList();
            modell.masaKatList = db.masaKategoris.ToList();
            modell.masaList = db.Masas.ToList();

            return View(modell);
        }
  

        public ActionResult MasaDetay(int masaID)
        {
          
           

            IEnumerable<CartItem> veri = null;

            veri = db.CartItems.GroupBy(cust => cust.cartNo).Select(m => m.FirstOrDefault());
          


            var cartlar = veri.Where(x => x.masaID == masaID && x.siparisonay == false).OrderByDescending(m => m.cartItemID).ToList();
            return View(cartlar);

            
        }
        public ActionResult SiparisDetay(int cartNo)
        {
            CartItem model = new CartItem();


            model.cartlar = db.CartItems.Where(x => x.cartNo == cartNo).GroupBy(cust => cust.cartNo).Select(m => m.FirstOrDefault());

            model.cartt = db.CartItems.Where(x => x.cartNo == cartNo).ToList();
            ViewBag.sizeID = new SelectList(db.SizeProducts, "sizeID", "sizeName");

            return View(model);
        }
    }
}