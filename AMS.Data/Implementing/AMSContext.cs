using AMS.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AMS.Data.Implementing
{
    public class AMSContext : DbContext
    {
        public AMSContext(DbContextOptions<AMSContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typesToRegister = from t in Assembly.GetExecutingAssembly().GetTypes()
                                  where !string.IsNullOrEmpty(t.Namespace) &&
                                        t.BaseType != null &&
                                        t.BaseType.IsGenericType
                                  let genericType = t.BaseType.GetGenericTypeDefinition()
                                  where genericType == typeof(MapBase<>)
                                  select t;

            foreach (var type in typesToRegister)
            {
                var instance = Activator.CreateInstance(type);
                type.GetMethod("Map").Invoke(instance, new object[] { modelBuilder });
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
