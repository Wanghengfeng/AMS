using AMS.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Data.Mapping
{
    public class AccountRoleMap : MapBase<AccountRole>
    {
        public override Action<EntityTypeBuilder<AccountRole>> BuilderAction { get; }

        public AccountRoleMap()
        {
            BuilderAction = entry =>
            {
                entry.HasKey(t => t.Id);

                // Properties
                // Table & Column Mappings
                entry.ToTable("AccountRole");

                //Relationships
                entry.HasOne(i => i.Account).WithMany(a => a.AccountRoles).HasForeignKey(i => i.AccountId);
                entry.HasOne(i => i.Role).WithMany().HasForeignKey(i => i.RoleId);
            };
        }
    }
}
