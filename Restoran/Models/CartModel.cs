using ibernsof_Restoran.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ibernsof_Restoran.Models
{
    public class CartModel
    {
        public Urun Urun { get; set; }
        public ExtraUrun ExtraUrun { get; set; }
        public SizeProduct SizeProduct { get; set; }
        public int Quantity { get; set; }
        public decimal NewPrice { get; set; }
        public decimal EklePrice { get; set; }


        public Nullable<System.DateTime> zaman { get; set; }
        public int cartID { get; set; }
        public Int64 cartNumber { get; set; }
        public int urunMiktar { get; set; }
      
        public int masaID{ get; set; }
        public int cartNo { get; set; }
        public string  odemeTur { get; set; }
        public string urunMesaj { get; set; }
        public int urunID { get; set; }
        public string extraUrunAdi { get; set; }
        public string sizeName { get; set; }

        public Nullable<int> sizeID { get; set; }



    }
}