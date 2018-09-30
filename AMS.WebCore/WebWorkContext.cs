using AMS.Core;
using AMS.Data.Domain;
using AMS.Services.Customers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.WebCore
{
    public class WebWorkContext: IWorkContext
    {
        #region ctor
        private Account _account;
        private readonly IAccountService _accountService;
        public WebWorkContext(IHttpContextAccessor httpContext, IAccountService accountService)
        {
            CurrentHttpContext = httpContext.HttpContext;
            _accountService = accountService;
        }
        #endregion

        public HttpContext CurrentHttpContext { get; }

        public Account CurrentAccount
        {
            get
            {
                if (_account == null)
                {
                    if (CurrentHttpContext.User.Identity.IsAuthenticated)
                    {
                        _account = _accountService.GetByUserName(CurrentHttpContext.User.Identity.Name);
                        return _account;
                    }
                }
                else
                {
                    return _account;
                }
                return null;
            }
            set
            {
                _account = value;
            }
        }
    }
}
