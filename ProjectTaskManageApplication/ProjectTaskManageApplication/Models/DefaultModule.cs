using Autofac;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WorkingTask.Services.Documents;

namespace ProjectTaskManageApplication.Models
{
    public class DefaultModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DocumentService>().As<IDocumentService>();
            //builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();


            //var dataAccess = Assembly.GetExecutingAssembly();
            //builder.RegisterAssemblyTypes(dataAccess)
            //    .Where(t => t.Name.EndsWith("Service"))
            //    .AsImplementedInterfaces()
            //    .InstancePerMatchingLifetimeScope();
        }
    }
}
