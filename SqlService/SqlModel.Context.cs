﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SqlService
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class courseworkEntities : DbContext
    {
        public courseworkEntities()
            : base("name=courseworkEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<account> account { get; set; }
        public virtual DbSet<details> details { get; set; }
        public virtual DbSet<eduprogram> eduprogram { get; set; }
        public virtual DbSet<faculty> faculty { get; set; }
        public virtual DbSet<group> group { get; set; }
        public virtual DbSet<reply> reply { get; set; }
        public virtual DbSet<replyCollection> replyCollection { get; set; }
        public virtual DbSet<user> user { get; set; }
    }
}