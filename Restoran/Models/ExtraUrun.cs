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
    
    public partial class ExtraUrun
    {
        public int extraUrunID { get; set; }
        public string extraUrunAdi { get; set; }
        public Nullable<decimal> extraUrunFiyat { get; set; }
        public Nullable<int> urunID { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<bool> isSelected { get; set; }
        public Nullable<int> extraUrunAdet { get; set; }
    
        public virtual Urun Urun { get; set; }
    }
}
