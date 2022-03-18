using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Core.Domain.DomainModels;

namespace ATS.Data.EF.Mappings
{
    public class UserAccount_Mapping : EntityTypeConfiguration<UserAccount>
    {
        public UserAccount_Mapping()
        {
            this.ToTable("UserAccount");
            this.HasKey(x => x.ID);
            this.Property(x => x.UserName).HasColumnName("UserName");
            this.Property(t => t.RoleID).HasColumnName("RoleID");            
            this.Property(t => t.PasswordHash).HasColumnName("PasswordHash");
            this.Property(t => t.Salt).HasColumnName("Salt");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");

        }
    }
}
