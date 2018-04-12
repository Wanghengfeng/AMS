using AMS.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace AMS.Data.Mapping
{
    public class PermissionMap : MapBase<Permission>
    {
        public override Action<EntityTypeBuilder<Permission>> BuilderAction { get; }

        public PermissionMap()
        {
            BuilderAction = entry =>
            {
                entry.HasKey(t => t.Id);

                // Properties
                // Table & Column Mappings
                entry.ToTable("Permission");
            };
        }
    }
}
