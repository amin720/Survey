﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Survey.Core.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SurveyEntities : DbContext
    {
        public SurveyEntities()
            : base("name=SurveyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Statistic> Statistics { get; set; }
        public virtual DbSet<TBL_Answers> TBL_Answers { get; set; }
        public virtual DbSet<TBL_Questions> TBL_Questions { get; set; }
        public virtual DbSet<TBL_Respondents> TBL_Respondents { get; set; }
        public virtual DbSet<TBL_RSQA> TBL_RSQA { get; set; }
        public virtual DbSet<TBL_Sections> TBL_Sections { get; set; }
        public virtual DbSet<TBL_Surveys> TBL_Surveys { get; set; }
    }
}
