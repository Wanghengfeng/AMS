﻿using AMS.Core;
using Microsoft.Extensions.Options;
using System;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using AMS.Model;

namespace AMS.Services.Authentication
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        #region ctor
        private readonly IWorkContext _workContext;
        private readonly AppSettings _appSettings;
        public FormsAuthenticationService(IWorkContext workContext, IOptionsMonitor<AppSettings> options)
        {
            _workContext = workContext;
            _appSettings = options.CurrentValue;
        }
        #endregion

        public void SignIn(string userName)
        {
            var claims = new Claim[] { new Claim(ClaimTypes.Name, userName) };
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            _workContext.CurrentHttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(_appSettings.CookieTimeout),
                IsPersistent = _appSettings.CookieIsPersistent,
                AllowRefresh = false,
            });
        }

        public void SignOut()
        {
            _workContext.CurrentHttpContext.SignOutAsync();
        }
    }
}
