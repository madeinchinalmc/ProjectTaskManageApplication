using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WorkingTask.Services
{
    public static class MyHttpContext
    {
        public static HttpContext Current
        {
            get
            {
                object factory = ServiceProvider.GetService(serviceType: typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));

                HttpContext httpContent = ((IHttpContextAccessor)factory).HttpContext;
                return httpContent;
            }
        }
    }
}
