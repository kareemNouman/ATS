using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;

namespace ATS.Data.EF.Mappings
{
    public class Attendance_Mapping : EntityTypeConfiguration<Attendance>
    {
        public Attendance_Mapping()
        {
            this.ToTable("Attendance");

            this.HasKey(x => x.ID);
            this.Property(x => x.EmployeeCode).HasColumnName("EmployeeCode");
            this.Property(x => x.Name).HasColumnName("Name");
            this.Property(x => x.Designation).HasColumnName("Designation");
            this.Property(x => x.Department).HasColumnName("Department");
            this.Property(x => x.TimeIn).HasColumnName("TimeIn");
            this.Property(x => x.TimeOut).HasColumnName("TimeOut");
            this.Property(x => x.TotalHours).HasColumnName("TotalHours");
            this.Property(x => x.OT1).HasColumnName("OT1");
            this.Property(x => x.OT2).HasColumnName("OT2");
            this.Property(x => x.OT3).HasColumnName("OT3");
            this.Property(x => x.OT4).HasColumnName("OT4");
            this.Property(x => x.Date).HasColumnName("Date");
            this.Property(x => x.Status).HasColumnName("Status");
            this.Property(x => x.IsActive).HasColumnName("IsActive");            
            this.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            this.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
        }

    }
}
