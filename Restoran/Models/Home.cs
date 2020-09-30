using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ibernsof_Restoran.Models
{
    public class Home
    {
   

        //public IEnumerable<CartItem> cartlar { get; set; } Cartitem
        //public IEnumerable<CartItem> cartt { get; set; }

        //public IEnumerable<masaKategori> masaKatList { get; set; }MasaKategori
        //public IEnumerable<Masa> masaList { get; set; }  MasaKategori

        //public IEnumerable<AltKategori> altkat { get; set; } Kategori

        //public IEnumerable<Urun> singleurun { get; set; } Urun
        //public IEnumerable<SizeProduct> sizeurun { get; set; }
        //public IEnumerable<ExtraUrun> extraurunler { get; set; }
        public IEnumerable<CartItem> tutar { get; set; }
        public IEnumerable<CartItem> masaID { get; set; }
        public IEnumerable<CartItem> siparisonay { get; set; }

        public IEnumerable<CarouselOne> header { get; set; }
        public IEnumerable<Urun> urunStok { get; set; }
        public IEnumerable<CarouselOne> onelist { get; set; }

        public IEnumerable<CarouselTwo> headerT { get; set; }
        public IEnumerable<CarouselTwo> listT { get; set; }
    }
}