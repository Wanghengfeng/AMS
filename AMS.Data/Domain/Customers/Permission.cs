using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Data.Domain
{
    public class Permission : EntityBase
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }
    }
}
