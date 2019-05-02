using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project3.Models.Drivers
{
    public class TimesDriver
    {
        public Nullable<System.DateTime> ConfirmationTime { get; set; }
        public int DriverKey { get; set; }
        public Nullable<System.DateTime> DeliveredTime { get; set; }
        public Nullable<System.DateTime> PickedupTime { get; set; }
        public Nullable<System.DateTime> OpenTime { get; set; }
        public Nullable<System.DateTime> DispatchTime { get; set; }
        public Nullable<System.DateTime> CloseTime { get; set; }
        //public TimesDriver(TimesDriver fact)
        //{
        //    this.ConfirmationTime = fact.ConfirmationTime;
        //    this.DeliveredTime = fact.DeliveredTime;
        //    this.PickedupTime = fact.PickedupTime;
        //}
        public TimesDriver()
        {

        }
    }
}