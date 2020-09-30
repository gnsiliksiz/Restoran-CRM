using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ibernsof_Restoran.Models
{
    public class EndOfDayModel
    {
        public string urunAd { get; set; }
        public int urunToplamMiktar { get; set; }
        public double? urunToplamFiyat { get; set; }


    }
}