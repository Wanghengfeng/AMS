using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Services.Authentication
{
    public interface IAuthenticationService
    {
        void SignIn(string userName);

        void SignOut();
    }
}
