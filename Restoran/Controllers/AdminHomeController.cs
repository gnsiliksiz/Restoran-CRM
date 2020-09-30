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
    public class AdminHomeController : Controller
    {
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        // GET: AdminHome
        public ActionResult Index()
        {
            Home model = new Home();
          
            model.siparisonay = db.CartItems.Where(x => x.siparisonay == true).ToList();
            model.tutar = db.CartItems.Where(x => x.onay == true).ToList();
            model.header = db.CarouselOnes.GroupBy(cust => cust.reklamHeader).Select(m => m.FirstOrDefault());

            model.onelist = db.CarouselOnes.ToList();

            model.headerT = db.CarouselTwoes.GroupBy(cust => cust.reklamtwoHeader).Select(m => m.FirstOrDefault());

            model.listT = db.CarouselTwoes.ToList();
            model.urunStok = db.Uruns.Where(x => x.urunStok <= 10).OrderBy(c=>c.urunStok).ToList();

            return View(model);


            ////var cartla= db.CartItems.Where(x => x.onay == true).ToList();



            //return View(cartla);
           

        }
    }
}