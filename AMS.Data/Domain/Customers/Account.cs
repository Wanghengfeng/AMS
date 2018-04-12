using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Data.Domain
{
    public class Account : EntityBase
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime LastLoginTime { get; set; }

        public string LastLoginIp { get; set; }

        public ICollection<AccountRole> AccountRoles { get; set; }
    }
}
