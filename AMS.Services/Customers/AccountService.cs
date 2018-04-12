using AMS.Data.Domain;
using AMS.Data.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Services.Customers
{
    public class AccountService:IAccountService
    {
        #region ctor
        private readonly IDbAccessor _amsAccessor;
        public AccountService(IDbAccessor amsAccessor)
        {
            _amsAccessor = amsAccessor;
        }
        #endregion

        #region account
        public Account GetById(int id)
        {
            return _amsAccessor.GetById<Account>(id);
        }

        public Account GetByUserName(string userName)
        {
            return _amsAccessor.Get<Account>().FirstOrDefault(i => i.UserName == userName && i.Isvalid);
        }

        public void Update(Account account)
        {
            _amsAccessor.Update(account);
            _amsAccessor.SaveChanges();
        }

        public void UpdateAccountRole(int id, IEnumerable<int> roleIds)
        {
            var olds = _amsAccessor.Get<AccountRole>(i => i.AccountId == id && !roleIds.Contains(i.RoleId));
            _amsAccessor.DeleteRange(olds);
            var news = roleIds.Where(i => !_amsAccessor.Get<AccountRole>().Any(j => j.AccountId == id && j.RoleId == i))
                .Select(i => new AccountRole { AccountId = id, RoleId = i });
            _amsAccessor.InsertRange(news);
            _amsAccessor.SaveChanges();
        }

        public void Create(Account account)
        {
            _amsAccessor.Insert(account);
            _amsAccessor.SaveChanges();
        }

        public IQueryable<Account> GetList()
        {
            return _amsAccessor.Get<Account>(i => i.Isvalid).Include(i => i.AccountRoles).ThenInclude(ar => ar.Role);
        }

        public IQueryable<Account> GetList(int roleId)
        {
            return GetList();
        }

        public void Delete(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return;
            }
            foreach (var i in ids)
            {
                Delete(i);
            }
            _amsAccessor.SaveChanges();
        }

        public void Delete(int id)
        {
            var account = GetById(id);
            account.Isvalid = false;
            _amsAccessor.Update(account);
        }

        #endregion

        #region role
        public Role GetRoleById(int id)
        {
            return _amsAccessor.GetById<Role>(id);
        }

        public Role GetRoleByCode(string code)
        {
            return _amsAccessor.Get<Role>().FirstOrDefault(i => i.Code == code && i.Isvalid);
        }

        public void UpdateRole(Role role)
        {
            _amsAccessor.Update(role);
            _amsAccessor.SaveChanges();
        }

        public void CreateRole(Role role)
        {
            _amsAccessor.Insert(role);
            _amsAccessor.SaveChanges();
        }

        public IQueryable<Role> GetRoleList()
        {
            return _amsAccessor.Get<Role>(i => i.Isvalid);
        }

        public IQueryable<Role> GetRoleList(int accountId)
        {
            var roleIds = _amsAccessor.Get<AccountRole>(i => i.AccountId == accountId).Select(i => i.RoleId);
            return _amsAccessor.Get<Role>(i => roleIds.Contains(i.Id));
        }

        public IQueryable<Permission> GetPermissionByRole(int roleId)
        {
            return _amsAccessor.Get<RolePermission>(i => i.RoleId == roleId)
                .Include(i => i.Permission).Select(i => i.Permission);
        }

        public void UpdateRolePermission(int roleId, IEnumerable<int> permissionIds)
        {
            var deletes = _amsAccessor.Get<RolePermission>(i => i.RoleId == roleId && !permissionIds.Contains(i.PermissionId));
            _amsAccessor.DeleteRange(deletes);
            var inserts = permissionIds.Where(i => !_amsAccessor.Get<RolePermission>().Any(j => j.RoleId == roleId && j.PermissionId == i))
                .Select(i => new RolePermission { PermissionId = i, RoleId = roleId });
            _amsAccessor.InsertRange(inserts);
            _amsAccessor.SaveChanges();
        }
        #endregion
    }
}
