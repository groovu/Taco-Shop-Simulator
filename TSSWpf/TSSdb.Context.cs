﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TSSWpf
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TacoDBEntity : DbContext
    {
        public TacoDBEntity()
            : base("name=TacoDBEntity")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ingredient> ingredients { get; set; }
        public virtual DbSet<login> logins { get; set; }
        public virtual DbSet<recipe> recipes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<test> tests { get; set; }
        public virtual DbSet<userData> userDatas { get; set; }
    }
}
