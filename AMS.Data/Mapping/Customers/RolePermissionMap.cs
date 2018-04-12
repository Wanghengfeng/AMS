using AMS.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Data.Mapping
{
    public class RolePermissionMap : MapBase<RolePermission>
    {
        public override Action<EntityTypeBuilder<RolePermission>> BuilderAction { get; }

        public RolePermissionMap()
        {
            BuilderAction = entry =>
            {
                entry.HasKey(t => t.Id);

                // Properties
                // Table & Column Mappings
                entry.ToTable("RolePermission");

                //Relationships
                entry.HasOne(i => i.Role).WithMany().HasForeignKey(i => i.RoleId);
                entry.HasOne(i => i.Permission).WithMany().HasForeignKey(i => i.PermissionId);
            };
        }
    }
}
