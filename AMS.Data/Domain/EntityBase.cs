using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Data.Domain
{
    public abstract class EntityBase
    {
        public EntityBase()
        {
            Isvalid = true;
            CreatedTime = DateTime.Now;
            ModifiedTime = DateTime.Now;
        }

        public virtual int Id { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime ModifiedTime { get; set; }

        public bool Isvalid { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
