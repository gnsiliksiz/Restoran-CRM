using System;

namespace ibernsof_Restoran.Models
{
    public class NotificationModel
    {
        public string BildirimTur { get; set; }

        public string bildirimIcerik { get; set; }
        
        public bool BildirimDurum { get; set; }

        public Nullable<System.DateTime> BildirimDate { get; set; }

       
    }
}