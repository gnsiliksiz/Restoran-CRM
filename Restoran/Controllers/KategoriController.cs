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
    public class KategoriController : Controller
    {
        // GET: Kategori
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();

        [Route("Kategori/{isim}/{kategoriID}")]
        public ActionResult Index(int kategoriID)
        {
            
            Kategori model = new Kategori();


            model.altkat = db.AltKategoris.Where(x => x.isDeleted == false && x.kategoriID == kategoriID).OrderBy(s => s.altKatsira).ToList();

         

            List<CartModel> carts = (List<CartModel>)Session["cart"];
            int count = 0;
            if (carts != null)
            {
                count = carts.Count();
                decimal totalprice = 0;


                foreach (var item in carts)
                {
                  

                        totalprice += (decimal)((decimal)item.Quantity * item.NewPrice);

                  
                }


                ViewBag.totalprice = Math.Round(totalprice, 2);
                ViewBag.carts = carts;
                ViewBag.count = count;
            }

            return View(model);
        }


        public PartialViewResult GetMenu()
        {
            var kategoriler = db.Kategoris.Where(x => x.isDeleted == false ).ToList();
            return PartialView(kategoriler);
        }

        
        public JsonResult AddToCart(int urunID, int urunMiktar, int? sizeID, string urunmesaj, string values)
        {
            string result = "success";
            var sizeFiyat = db.SizeProducts.Where(x => x.sizeID == sizeID && x.urunID == urunID).Select(s=>s.sizeFiyat).FirstOrDefault();

            var urunFiyat = db.Uruns.Where(x => x.urunID == urunID).Select(s=>s.urunFiyat).FirstOrDefault();

            decimal degisen = 0;

            if(sizeFiyat==null || sizeFiyat==0)
            {
                degisen = urunFiyat;
            }
            else
            {
                degisen = sizeFiyat;
            }

            if (!String.IsNullOrEmpty(Session["qrHash"].ToString()))
            {
                var qrHash = Session["qrhash"].ToString();
                var masaa = db.Masas.Where(x => x.qrHash == qrHash).Select(x => x.masaID).FirstOrDefault();

                int masaID = Convert.ToInt32(masaa);


                var urun = db.Uruns.Find(urunID);


                if (Session["cart"] == null)
                {
                    List<CartModel> cartModels = new List<CartModel>();
                    cartModels.Add(new CartModel { Urun = urun, masaID = masaID, extraUrunAdi = values,NewPrice= degisen, sizeID = sizeID, urunMesaj = urunmesaj, Quantity = urunMiktar, zaman = DateTime.Now });
                    Session["cart"] = cartModels;
                }
                else
                {
                    List<CartModel> cartModels = (List<CartModel>)Session["cart"];
                    CartModel sameCart = cartModels.Where(t => t.Urun.urunID == urun.urunID).FirstOrDefault();
                    CartModel sizecart = cartModels.Where(t => t.Urun.urunID == urun.urunID && t.sizeID==sizeID).FirstOrDefault();

                    if (sameCart != null)
                    {
                        var urunStok = db.Uruns.Where(x => x.urunID == urunID).Select(m => m.urunStok).FirstOrDefault();
                        urunStok = Convert.ToInt32(urunStok);
                        int yeniMiktar = urunMiktar + sameCart.Quantity;
                     
                        if (yeniMiktar > urunStok)
                        {
                            
                            result = "fail";
                        }
                        else
                        {
                            if (sizeFiyat == null || sizeFiyat == 0 && sameCart==null)
                            {
                                sameCart.Quantity += urunMiktar;
                            }
                            else
                            {
                                if(sizecart!=null)
                                {
                                    sizecart.Quantity += urunMiktar;
                                }
                                else { 
                                
                                cartModels.Add(new CartModel { Urun = urun, masaID = masaID, extraUrunAdi = values, NewPrice = degisen, sizeID = sizeID, urunMesaj = urunmesaj, Quantity = urunMiktar, zaman = DateTime.Now });
                                }
                            }
                           
                            

                        }

                    }
                    else
                    {
                        cartModels.Add(new CartModel { Urun = urun, masaID = masaID, extraUrunAdi = values, NewPrice = degisen, sizeID = sizeID, urunMesaj = urunmesaj, Quantity = urunMiktar, zaman = DateTime.Now });
                    }
                  //  string a = JsonConvert.SerializeObject(cartModels);
                    Session["cart"] = cartModels;
                }


            }
         
            return Json(result);

        }


    

      
        public JsonResult AddNotification(List<NotificationModel> model)
        {
            try
            {
                var bildirimId = 0;
                using (var dbEntities = new ibernsof_RestoranEntities())
                {
                    foreach (var item in model)
                    {
                        Bildirim bildirim = new Bildirim()
                        {
                            bildirimTur = item.BildirimTur,
                            bildirimDurum = item.BildirimDurum,
                        
                            bildirimDate = DateTime.Now

                        };

                        dbEntities.Bildirims.Add(bildirim);

                    }
                    dbEntities.SaveChanges();

                    bildirimId = dbEntities.Bildirims.OrderByDescending(q => q.bildirimID).FirstOrDefault().bildirimID;

                }
                return Json(bildirimId);
            }
            catch (Exception)
            {

            }
            return null;
        }
        public JsonResult DeleteCarts( )
        {
            Session.Remove("cart");
            Session["cart"] = null;
            return Json("");
        }

        public ActionResult productDetail(int urunID,int? sizeID)
        {
            Urun model = new Urun();

         
            model.singleurun = db.Uruns.Where(x => x.urunID == urunID).GroupBy(cust => cust.urunID).Select(m => m.FirstOrDefault());

       

            model.extraurunler = db.ExtraUruns.Where(x => x.urunID == urunID).ToList();
            model.sizeurun = db.SizeProducts.Where(x => x.urunID == urunID).ToList();
            
            List<CartModel> carts = (List<CartModel>)Session["cart"];
            int count = 0;
            if (carts != null)
            {
                count = carts.Count();
                decimal totalprice = 0;

                var sizeFiyat = db.SizeProducts.Where(x => x.sizeID == sizeID && x.urunID == urunID).Select(m => m.sizeFiyat).FirstOrDefault();

                

                foreach (var item in carts)
                    {
                        totalprice += (decimal)((decimal)item.Quantity * item.NewPrice);
                    
                   
                }

                Session.Remove("sizeFiyat");
                Session.Remove("sizeID");
                ViewBag.totalprice = Math.Round(totalprice, 2);
                ViewBag.carts = carts;
                ViewBag.count = count;
            }
         


            return View(model);
          
        }

        public JsonResult productStock(int urunID)
        {

            var urunStok = db.Uruns.Where(x => x.urunID == urunID).Select(m => m.urunStok).FirstOrDefault();
            urunStok = Convert.ToInt32(urunStok);

            var result = urunStok;

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult productPrice(int urunID,int sizeID)
        {

            var sizeFiyat = db.SizeProducts.Where(x => x.sizeID == sizeID && x.urunID==urunID).Select(m => m.sizeFiyat).FirstOrDefault();
            var urunFiyat = db.Uruns.Where(x => x.urunID == urunID).Select(m => m.urunFiyat).FirstOrDefault();
              urunFiyat= Convert.ToDecimal(sizeFiyat);
            Session["sizeFiyat"] = urunFiyat;
            Session["sizeID"] = sizeID;

            var result = urunFiyat;
            
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}