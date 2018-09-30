
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Controllers
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CacheAttribute : Attribute, IActionFilter
    {
        public CacheAttribute(string name)
        {
            CacheName = name;
        }
        public string CacheName { get; set; }
        public void OnActionExecuted(ActionExecutedContext context)
        {
          
            //var key = $"{context.ServiceMa}"
            //context.Result = new ContentResult()
            //{
            //    Content = "success"
            //};
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //context.Result = new ContentResult()
            //{
            //    Content = "success"
            //};
        }
    }

    //public class CustomInterceptorAttribute : InterceptorAttribute
    //{
    //    public async override Task Invoke(AspectContext context, AspectDelegate next)
    //    {
    //        try
    //        {
    //            Console.WriteLine("Before service call");
    //            await next(context);
    //        }
    //        catch (Exception)
    //        {
    //            Console.WriteLine("Service threw an exception!");
    //            throw;
    //        }
    //        finally
    //        {
    //            Console.WriteLine("After service call");
    //        }
    //    }
    //}
}
