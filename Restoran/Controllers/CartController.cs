using ibernsof_Restoran.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Configuration;

namespace ibernsof_Restoran.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        // GET: OdemeTip
        public ActionResult Index()
        {
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

            return View();
        }

        public ActionResult Complete()
        {
           
            return View();

        }
        public ActionResult AddSepet(string odemeTur)
        {
            List<CartModel> carts = (List<CartModel>)Session["cart"];
            List<CartItem> sepets = new List<CartItem>();
          
            decimal totalPrice = 0;
            if (carts != null)
            {
                int cartNo = new Random().Next(1, 999999999);

                foreach (var item in carts)
                {
                  

                        totalPrice = (decimal)((decimal)item.Quantity * item.NewPrice);
                   
                    item.odemeTur=odemeTur;
             
                Session["cartNo"] = cartNo;


                    sepets.Add(new CartItem
                    {
                        extraUrunAdi = item.extraUrunAdi,
                        urunMesaj = item.urunMesaj,
                        urunSizeID = item.sizeID,
                        masaID = item.masaID,
                        onay = false,
                        active = true,
                        urunID = item.Urun.urunID,
                        cartNo = cartNo,
                        fiyat = @Math.Round((decimal)totalPrice, 2),
                        miktar = item.Quantity,
                        odemeTur = item.odemeTur,
                        siparisonay = false,


                        zaman = DateTime.Now

                    }); 
                }
                 
                db.CartItems.AddRange(sepets);

                db.SaveChanges();

                return RedirectToAction("Complete");
            }

            return RedirectToAction("Complete", "Cart");

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
            catch (Exception ex)
            {


            }
            return null;
        }

        public ActionResult DeleteUrun()
        {

            Session.Remove("cart");
            Session["cart"] = null;
            return Redirect("");
        }
       
    }
}