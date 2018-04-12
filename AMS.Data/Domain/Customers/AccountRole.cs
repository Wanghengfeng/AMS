using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Data.Domain
{
    public class AccountRole
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

        public Account Account { get; set; }
    }
}
