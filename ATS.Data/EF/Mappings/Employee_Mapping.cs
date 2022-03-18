using ATS.Core.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Data.EF.Mappings
{
    public class Employee_Mapping : EntityTypeConfiguration<Employee>
    {
        public Employee_Mapping()
        {
            this.ToTable("Employee");

            this.HasKey(x => x.ID);            
            this.Property(x => x.EmployeeCode).HasColumnName("EmployeeCode");
            this.Property(x => x.Name).HasColumnName("Name");
            this.Property(x => x.Email).HasColumnName("Email");
            this.Property(x => x.DepartmentID).HasColumnName("DepartmentID");
            this.Property(x => x.DesignationID).HasColumnName("DesignationID");
            this.Property(x => x.Basic).HasColumnName("Basic");
            this.Property(x => x.SplAllowance).HasColumnName("SplAllowance");
            this.Property(x => x.Col).HasColumnName("Col");
            this.Property(x => x.OthersAllowance).HasColumnName("OthersAllowance");
            this.Property(x => x.Conveyance).HasColumnName("Conveyance");
            this.Property(x => x.Housing).HasColumnName("Housing");
            this.Property(x => x.Gross).HasColumnName("Gross");
            this.Property(x => x.OTThreshold).HasColumnName("OTThreshold");
            this.Property(x => x.WeekOffMain).HasColumnName("WeekOffMain");
            this.Property(x => x.WeeklyOffAlternate).HasColumnName("WeeklyOffAlternate");
            this.Property(x => x.IsOTEligible).HasColumnName("IsOTEligible");
            this.Property(x => x.DORJ).HasColumnName("DORJ");
            //this.Property(x => x.IsActive).HasColumnName("IsActive");
            this.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            this.Property(x => x.CreatedOn).HasColumnName("CreatedOn");

        }
    }
}
