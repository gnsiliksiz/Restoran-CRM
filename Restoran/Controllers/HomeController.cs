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
    
    public class HomeController : Controller
    {
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        // GET: Home
        public ActionResult Index()
        {

            if (!String.IsNullOrEmpty(Request.QueryString["qrhash"]))
            {
                // Query string value is there so now use it
                string qrHash = Request.QueryString["qrhash"];
               
                Session["qrHash"] = qrHash;
                var masaNo = db.Masas.Where(x => x.qrHash == qrHash).Select(x => x.masaNo).FirstOrDefault();
                Session["MASANO"] = masaNo.ToString();

            }
            return View();
        }

    }
}