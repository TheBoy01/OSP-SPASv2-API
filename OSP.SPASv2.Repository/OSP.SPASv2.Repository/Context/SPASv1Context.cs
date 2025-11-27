using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SPASv2.Context.SPASv2Context;

namespace OSP.SPASv2.Repository.Context
{
    public class SPASv1Context : DbContext
    {

        //private readonly string _dynamicConnectionString;

        public SPASv1Context(DbContextOptions<SPASv1Context> options) : base(options)
        {
            //_dynamicConnectionString = dynamicConnectionString;
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(_dynamicConnectionString);
        //        // Use other database providers if needed (e.g., UseMySQL, UsePostgreSQL, etc.)
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ValReturn<bool>>().HasNoKey();
            modelBuilder.Entity<ValReturn<int>>().HasNoKey();
            modelBuilder.Entity<ValReturn<DateTime>>().HasNoKey();
            modelBuilder.Entity<ValReturn<string>>().HasNoKey();
            modelBuilder.Entity<ValReturn<decimal>>().HasNoKey();

            #region Tables

            modelBuilder.Entity<TblDataSourceHdr>().HasKey(t => new { t.BatchName,t.ReferenceNo });
            modelBuilder.Entity<TblDataSourceDtl>().HasKey(t => new { t.Idx });

            #endregion

            #region References

            #endregion

            #region Views

            #endregion


        }


        #region Tables

        public DbSet<TblDataSourceHdr> TblDataSourceHdr { get; set; }
        public DbSet<TblDataSourceDtl> TblDataSourceDtl { get; set; }

        #endregion

    }
}
