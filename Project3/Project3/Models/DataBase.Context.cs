﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DimDate> DimDates { get; set; }
        public virtual DbSet<DimDish> DimDishes { get; set; }
        public virtual DbSet<DimDriver> DimDrivers { get; set; }
        public virtual DbSet<DimRestaurant> DimRestaurants { get; set; }
        public virtual DbSet<DimUser> DimUsers { get; set; }
        public virtual DbSet<FactBill> FactBills { get; set; }
        public virtual DbSet<OldDateDim> OldDateDims { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<FacebookComment> FacebookComments { get; set; }
        public virtual DbSet<FacebookPost> FacebookPosts { get; set; }
        public virtual DbSet<DishDataMining> DishDataMinings { get; set; }
        public virtual DbSet<UserDataMining> UserDataMinings { get; set; }
    }
}
