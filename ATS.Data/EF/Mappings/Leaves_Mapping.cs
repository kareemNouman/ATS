using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;

namespace ATS.Data.EF.Mappings
{
    public class Leaves_Mapping : EntityTypeConfiguration<Leaves>
    {
        public Leaves_Mapping()
        {
            this.ToTable("Leaves");

            this.HasKey(x => x.ID);

            this.Property(x => x.Name).HasColumnName("Name");
            this.Property(x => x.IsDelete).HasColumnName("IsDelete");
        }

    }
}
