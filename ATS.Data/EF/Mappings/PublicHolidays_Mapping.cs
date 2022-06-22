using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;

namespace ATS.Data.EF.Mappings
{
    public class PublicHolidays_Mapping : EntityTypeConfiguration<PublicHolidays>
    {
        public PublicHolidays_Mapping()
        {
            this.ToTable("PublicHolidays");

            this.HasKey(x => x.ID);
            this.Property(x => x.Name).HasColumnName("Name");
            this.Property(x => x.Date).HasColumnName("Date");
            this.Property(x => x.IsDelete).HasColumnName("IsDelete");
        }
    }
}
