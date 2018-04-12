using AMS.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Services.Customers
{
    public interface IAccountService
    {
        Account GetById(int id);

        Account GetByUserName(string userName);

        void Update(Account account);

        void UpdateAccountRole(int id, IEnumerable<int> roleIds);

        void Create(Account account);

        void Delete(int id);

        void Delete(int[] ids);

        IQueryable<Account> GetList();

        IQueryable<Account> GetList(int roleId);

        Role GetRoleById(int id);

        Role GetRoleByCode(string code);


        void UpdateRole(Role role);

        void CreateRole(Role role);

        IQueryable<Role> GetRoleList();

        IQueryable<Role> GetRoleList(int accountId);

        IQueryable<Permission> GetPermissionByRole(int roleId);

        void UpdateRolePermission(int roleId, IEnumerable<int> permissionIds);
    }
}
