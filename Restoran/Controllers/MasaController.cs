using ibernsof_Restoran.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;

namespace ibernsof_Restoran.Controllers
{
    public class MasaController : Controller
    {
       ibernsof_RestoranEntities db = new ibernsof_RestoranEntities();
        public ActionResult Index()
        {
            int sess = Convert.ToInt32(Session["userID"]);
            var masalar = db.Masas.Where(x => x.userID == sess).ToList();
            return View(masalar);
        }
        public ActionResult Create()
        {
            ViewBag.masaKatID = new SelectList(db.masaKategoris, "masaKatID", "masaKatAdi");
            return View();
        }
        string url;

        [HttpPost]
        public ActionResult Create(Masa masa)
        {
            int sess = Convert.ToInt32(Session["userID"]);
            try
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[16];
                var random = new Random();
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                var finalString = new String(stringChars);
                masa.qrHash = finalString;

                Console.WriteLine("finalString:" + finalString);
                masa.qrFoto = GenerateQRCode(masa.qrHash, masa.masaNo);
                masa.userID = sess;
                masa.qrUrl = url;
                db.Masas.Add(masa);
                db.SaveChanges();
           
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "Masa");
        }
        private string GenerateQRCode(string qrcodeText, string masano)
        {
            string folderPath = "~/Uploads/QrImage/";
            string imagePath = "~/Uploads/QrImage/" + masano + ".jpeg";
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(Server.MapPath(folderPath)))
            {
                Directory.CreateDirectory(Server.MapPath(folderPath));
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            Session["masahash"] = qrcodeText.ToString();
            Session["masano"] = masano.ToString();
            //var result = barcodeWriter.Write("www.ibernsoft.com.tr/qrhash="+ qrcodeText);
            var result = barcodeWriter.Write("http://demo.ibernsoft.net/Home/Index?qrhash=" + qrcodeText);
            url = "http://demo.ibernsoft.net?qrhash=" + qrcodeText;
            string barcodePath = Server.MapPath(imagePath);
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return imagePath;
        }

        
        

        private Masa ReadQRCode()
        {
            Masa barcodeModel = new Masa();
            string barcodeText = "";
            string imagePath = "~/Uploads/QrImage/QrCode.jpg";
            string barcodePath = Server.MapPath(imagePath);
            var barcodeReader = new BarcodeReader();

            var result = barcodeReader.Decode(new Bitmap(barcodePath));
            if (result != null)
            {
                barcodeText = result.Text;
            }
            return new Masa() { masaNo = barcodeText, qrFoto = imagePath };
        }
        public JsonResult CheckMasaAvailability(string userdata)
        {
            System.Threading.Thread.Sleep(200);
            var SeachData = db.Masas.Where(x => x.masaNo == userdata).SingleOrDefault();
            if (SeachData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }

        }

        public JsonResult DeleteMasaRecord(int masaID)
        {
            bool result = false;
            Masa usr = db.Masas.Find( masaID);
            if (usr != null)
            {
                //db.Bildirims.Remove(usr);
                db.Masas.Remove(usr);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}