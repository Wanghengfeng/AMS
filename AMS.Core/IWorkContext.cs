using AMS.Data.Domain;
using Microsoft.AspNetCore.Http;

namespace AMS.Core
{
    public interface IWorkContext
    {
        HttpContext CurrentHttpContext { get; }

        Account CurrentAccount { get; set; }
    }
}
