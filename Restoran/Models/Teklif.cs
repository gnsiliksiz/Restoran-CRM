//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ibernsof_Restoran.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Teklif
    {
        public int teklifID { get; set; }
        public string teklifAdi { get; set; }
        public string teklifResim { get; set; }
        public Nullable<decimal> teklifFiyat { get; set; }
        public string teklifAciklama { get; set; }
        public Nullable<int> teklifMiktar { get; set; }
        public Nullable<int> teklifStok { get; set; }
        public Nullable<System.DateTime> teklifZaman { get; set; }
        public Nullable<int> teklifGecerliGunSayisi { get; set; }
        public Nullable<bool> active { get; set; }
        public string teklifHangiGun { get; set; }
    }
}
