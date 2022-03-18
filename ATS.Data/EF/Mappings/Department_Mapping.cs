using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;

namespace NEC.Data.EF.Mappings
{
    public class Department_Mapping : EntityTypeConfiguration<Department>
    {
        public Department_Mapping()
        {
            this.ToTable("Department");

            this.HasKey(x => x.ID);

            this.Property(x => x.Name).HasColumnName("Name");
            this.Property(x => x.IsDelete).HasColumnName("IsDelete");
        }

    }
}
