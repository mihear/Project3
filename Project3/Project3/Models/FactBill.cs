//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FactBill
    {
        public int Id { get; set; }
        public int BillKey { get; set; }
        public int UserKey { get; set; }
        public Nullable<int> DateKey { get; set; }
        public int ResturentKey { get; set; }
        public int DriverKey { get; set; }
        public Nullable<int> DishKey { get; set; }
        public string Status { get; set; }
        public Nullable<int> Paid { get; set; }
        public Nullable<int> BillType { get; set; }
        public Nullable<int> Rate { get; set; }
        public Nullable<double> SubTotal { get; set; }
        public Nullable<double> NetTotal { get; set; }
        public Nullable<double> Tax { get; set; }
        public Nullable<System.DateTime> OpenTime { get; set; }
        public Nullable<System.DateTime> HandlingTime { get; set; }
        public Nullable<double> ElapsedHandlingTime { get; set; }
        public string ExpectedTime { get; set; }
        public Nullable<System.DateTime> CloseTime { get; set; }
        public string TimeType { get; set; }
        public string TimeToPick { get; set; }
        public Nullable<double> Delivery { get; set; }
        public Nullable<double> Commision { get; set; }
        public Nullable<double> DueFromRestaurant { get; set; }
        public int BillDriverStatus { get; set; }
        public Nullable<int> Distance { get; set; }
        public Nullable<double> FactorDistance { get; set; }
        public Nullable<double> Late { get; set; }
        public Nullable<System.DateTime> ConfirmationTime { get; set; }
        public Nullable<System.DateTime> DispatchTime { get; set; }
        public Nullable<System.DateTime> DeliveredTime { get; set; }
        public Nullable<System.DateTime> PickedupTime { get; set; }
        public Nullable<System.DateTime> ArrivalTime { get; set; }
        public Nullable<System.DateTime> DepatureTime { get; set; }
        public Nullable<int> OrderQuantity { get; set; }
        public Nullable<double> OrderDishPrice { get; set; }
        public Nullable<double> OrderTotal { get; set; }
        public Nullable<short> OrderDeleted { get; set; }
    
        public virtual DimDate DimDate { get; set; }
        public virtual DimDish DimDish { get; set; }
        public virtual DimDriver DimDriver { get; set; }
        public virtual DimRestaurant DimRestaurant { get; set; }
        public virtual DimUser DimUser { get; set; }
    }
}
