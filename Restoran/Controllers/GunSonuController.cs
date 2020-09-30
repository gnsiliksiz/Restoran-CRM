using Newtonsoft.Json;
using ibernsof_Restoran.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ibernsof_Restoran.Controllers
{
    public class GunSonuController : Controller
    {
        // GET: GunSonu

        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        public ActionResult Index()
        {
            try
            {
                EndOfDayModel model = new EndOfDayModel();
                //DateTime dateTime = DateTime.UtcNow.Date;
                DateTime dateTime = DateTime.Today.Date;
                var siparisAdet = db.CartItems.Where(x => x.onay == true && System.Data.Entity.DbFunctions.TruncateTime(x.zaman) == dateTime.Date).Select(r => r.cartNo).Distinct().Count();

                var nakitTutar = db.CartItems.Where(x => x.onay == true && System.Data.Entity.DbFunctions.TruncateTime(x.zaman) == dateTime.Date && x.odemeTur == "nakit").Select(l => l.fiyat).DefaultIfEmpty(0).Sum();

                var posTutar = db.CartItems.Where(x => x.onay == true && System.Data.Entity.DbFunctions.TruncateTime(x.zaman) == dateTime.Date && x.odemeTur == "pos").Select(l => l.fiyat).DefaultIfEmpty(0).Sum();

                var toplamTutar = db.CartItems.Where(x => x.onay == true && System.Data.Entity.DbFunctions.TruncateTime(x.zaman) == dateTime.Date).Select(l => l.fiyat).DefaultIfEmpty(0).Sum();

                ViewBag.siparisAdet = siparisAdet;
                ViewBag.nakitTutar = nakitTutar; ;
                ViewBag.posTutar = posTutar;
                ViewBag.toplamTutar = toplamTutar;

                var products = db.CartItems.Where(x => x.onay == true).ToList();

                var list = from c in db.CartItems
                           join u in db.Uruns on c.urunID equals u.urunID
                           where c.onay == true && (System.Data.Entity.DbFunctions.TruncateTime(c.zaman) == dateTime.Date)
                           group new { c, u } by new { u.urunAd } into pg
                           let firstproductgroup = pg.FirstOrDefault()
                           let product = firstproductgroup.c
                           let baseproduct = firstproductgroup.u
                           let urunAd = baseproduct.urunAd
                           let toplamMiktar = pg.Sum(m => m.c.miktar)
                           let toplamFiyat = pg.Sum(m => m.c.fiyat)
                           select new
                           {
                               urunAd = urunAd,
                               toplamMiktar = toplamMiktar,
                               toplamFiyat = toplamFiyat,
                           };
                string test = JsonConvert.SerializeObject(list);
                List<EndOfDayModel> endOfDayModelList = new List<EndOfDayModel>();
                if (list.Count() == 0)
                {
                    ViewBag.Test = 0;
                    return View();

                }
                else
                {
                    foreach (var item in list)
                    {
                        double toplamFiyat2 = (double)item.toplamFiyat;
                        endOfDayModelList.Add(new EndOfDayModel { urunAd = item.urunAd, urunToplamFiyat = toplamFiyat2, urunToplamMiktar = item.toplamMiktar });
                    }
                    return View(endOfDayModelList);
                }

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + Server.HtmlEncode(ex.Message) + "')</script>");
                throw;
            }



        }

    }
}
