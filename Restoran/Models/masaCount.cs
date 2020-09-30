using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ibernsof_Restoran.Models
{
    public class masaCount
    {
        public int siparisCount { get; set; }
        public string masaNo { get; set; }
        public int masaID { get; set; }
        public Nullable<bool> siparisOnay { get; set; }
        public Nullable<int> masaKatID { get; set; }
        public string masaKatAdi { get; set; }
        public Masa Masa { get; set; }

    }
}