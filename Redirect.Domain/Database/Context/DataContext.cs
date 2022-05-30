using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Redirect.Domain.Database.Entities;
using Redirect.Domain.Database.Mapping;

namespace Redirect.Domain.Database.Context
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("[STRING DE CONEXÃO]")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        public DbSet<Register> Register { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RegisterMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}