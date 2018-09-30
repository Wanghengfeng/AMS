using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.WebCore.Controllers
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UserAnonymousAttribute : Attribute, IFilterMetadata
    {

    }
}
