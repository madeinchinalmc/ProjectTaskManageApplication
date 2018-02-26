using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace ProjectTaskManageApplication.Helper
{
    /// <summary>
    /// 获取程序集帮助类
    /// </summary>
    public class RuntimeHelper
    {
        /// <summary>
        /// 拿到所有程序集
        /// </summary>
        /// <returns></returns>
        public static IList<Assembly> GetAllAssemblies()
        {
            var list = new List<Assembly>();
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");//排除所有的系统程序集、Nuget下载包
            foreach(var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                catch (Exception)
                {

                }
            }
            return list;
        }
        public static Assembly GetAssembly(string assemblyName)
        {
            return GetAllAssemblies().FirstOrDefault(assembly => assembly.FullName.Contains(assemblyName));
        }
        public static IList<Type> GetAllTypes()
        {
            var list = new List<Type>();
            foreach(var assembly in GetAllAssemblies())
            {
                var typeInfos = assembly.DefinedTypes;
                foreach(var typeInfo in typeInfos)
                {
                    list.Add(typeInfo.AsType());
                }
            }
            return list;
        }
        public static IList<Type> GetTypesByAssembly(string assemblyName)
        {
            var list = new List<Type>();
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            var typeInfos = assembly.DefinedTypes;
            foreach(var typeInfo in typeInfos)
            {
                list.Add(typeInfo.AsType());
            }
            return list;
        }
        public static Type GetImplementType(string typeName,Type baseInterfaceType)
        {
            return GetAllTypes().FirstOrDefault(t =>
            {
                if (t.Name == typeName && t.GetTypeInfo().GetInterfaces().Any(b => b.Name == baseInterfaceType.Name))
                {
                    var typeInfo = t.GetTypeInfo();
                    return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericType;
                }
                return false;
            });
        }
    }
    /// <summary>
    /// IServiceCollection扩展
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// DI批量注入接口程序集中对应的实现类
        /// </summary>
        /// <param name="service"></param>
        /// <param name="interfaceAssemblyName">接口程序集的名称（不包含文件扩展名）</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAssembly(this IServiceCollection service,string interfaceAssemblyName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrEmpty(interfaceAssemblyName))
                throw new ArgumentNullException(nameof(interfaceAssemblyName));
            var assembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if (assembly == null)
                throw new DllNotFoundException($"the dll \"{interfaceAssemblyName}\" not be found");
            //过滤掉非接口以及泛型接口
            var types = assembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsGenericType);
            foreach(var type in types)
            {
                var implementTypeName = type.Name.Substring(1);
                var implementType = RuntimeHelper.GetImplementType(implementTypeName, type);
                if (implementType != null)
                    service.AddSingleton(type, implementType);
            }
            return service;
        }
        /// <summary>
        /// 用DI批量注入接口程序集中对应的实现类。
        /// </summary>
        /// <param name="service"></param>
        /// <param name="interfaceAssemblyName"></param>
        /// <param name="implementAssemblyName"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterAssembly(this IServiceCollection service, string interfaceAssemblyName,string implementAssemblyName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrEmpty(interfaceAssemblyName))
                throw new ArgumentNullException(nameof(interfaceAssemblyName));
            if (string.IsNullOrEmpty(implementAssemblyName))
                throw new ArgumentNullException(nameof(implementAssemblyName));
            var interfaceAssembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if(interfaceAssembly == null)
                throw new DllNotFoundException($"the dll \"{interfaceAssemblyName}\" not be found");
            var implementAssembly = RuntimeHelper.GetAssembly(implementAssemblyName);
            if (implementAssembly == null)
                throw new DllNotFoundException($"the dll \"{implementAssemblyName}\" not be found");
            //过滤掉非接口及泛型接口
            var types = interfaceAssembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsGenericType);
            foreach(var type in types)
            {
                var implementType = implementAssembly.DefinedTypes
                    .FirstOrDefault(t => t.IsClass && !t.IsAbstract && !t.IsGenericType &&
                    t.GetInterfaces().Any(b => b.Name == type.Name));
                if (implementType != null)
                    service.AddSingleton(type, implementType.AsType());
            }
            return service;
        }
    }
}
