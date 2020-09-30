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
    public class UrunWebController : Controller
    {
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();


        public ActionResult Detail(int id)
        {

            var prod = db.Uruns.Where(x => x.urunID == id).FirstOrDefault();
            return View(prod);
        }
    }
}