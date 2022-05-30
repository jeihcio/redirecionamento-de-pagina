using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Redirect.Domain.Database.Entities;

namespace Redirect.Domain.Database.Mapping
{
    public class RegisterMap: EntityTypeConfiguration<Register>
    {
        public RegisterMap()
        {
            ToTable("Registro");

            HasKey(x => x.Id);

            Property(x => x.Date)
                .IsRequired();

            Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(1000);

            Property(x => x.Guid)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}