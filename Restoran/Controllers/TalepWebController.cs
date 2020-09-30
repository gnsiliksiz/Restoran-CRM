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
    public class TalepWebController : Controller
    {
        ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        // GET: TalepWeb
        public ActionResult Index()
        {
            var d = db.MusteriIsteks.ToList();
            return View(d);
        }

        public PartialViewResult GetButton()
        {
            var d = db.MusteriIsteks.ToList();
            return PartialView(d);
        }

        public ActionResult EkleMusteriIstekRecord(int? musteriIstekID)
        {
            bool result = false;


            return View();
        }
    }
}