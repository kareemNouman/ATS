using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;

namespace ATS.Data.EF.Mappings
{
    public class EmployeeLeave_Mapping: EntityTypeConfiguration<EmployeeLeave>
    {
        public EmployeeLeave_Mapping()
        {
            this.ToTable("EmployeeLeave");

            this.HasKey(x => x.ID);
            this.Property(x => x.EmployeeCode).HasColumnName("EmployeeCode");
            this.Property(x => x.Name).HasColumnName("Name");
            this.Property(x => x.LeaveStart).HasColumnName("LeaveStart");
            this.Property(x => x.LeaveEnd).HasColumnName("LeaveEnd");
            this.Property(x => x.ExceedingDays).HasColumnName("ExceedingDays");
            this.Property(x => x.LeaveType).HasColumnName("LeaveType");
            this.Property(x => x.Remark).HasColumnName("Remark");
            this.Property(x => x.IsActive).HasColumnName("IsActive");
            this.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            this.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
        }

    }
}
