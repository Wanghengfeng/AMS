using AMS.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Data.Mapping
{
    public class RoleMap : MapBase<Role>
    {
        public override Action<EntityTypeBuilder<Role>> BuilderAction { get; }

        public RoleMap()
        {
            BuilderAction = entry =>
            {
                entry.HasKey(t => t.Id);

                // Properties
                // Table & Column Mappings
                entry.ToTable("Role");
            };
        }
    }
}
