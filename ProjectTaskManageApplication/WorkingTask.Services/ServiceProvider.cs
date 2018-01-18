using Autofac;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WorkingTask.Services
{
    public static class ServiceProvider
    {
        public static IServiceProvider Provider;
        static ServiceProvider()
        {
        }
        public static object GetService(Type serviceType)
        {
            return Provider.GetService(serviceType: serviceType);
        }
    }
}
