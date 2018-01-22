using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using ProjectTaskManageApplication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WorkingTask.Data;
using WorkingTask.Data.Models;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using ProjectTaskManageApplication.Controllers;
using Microsoft.AspNetCore.Http;
using WorkingTask.Services.Documents;
using ProjectTaskManageApplication.Filter;
using WorkingTask.Services.BaseDomain;
using System.Reflection;

namespace ProjectTaskManageApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var contextString = Configuration.GetConnectionString("DefaultConnection");
            //连接数据库服务加入DI容器
            services.AddDbContext<TaskManageContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            //将Identity服务加入DI容器
            services.AddIdentity<TaskManageUser, TaskManageUserRole>()
                .AddEntityFrameworkStores<TaskManageContext>()
                .AddDefaultTokenProviders();

            //设置注册密码的规则
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            var jwtSetting = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSetting);
            services.AddAuthentication(options =>
            {                                   // 添加认证头
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jo =>
            jo.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidIssuer = jwtSetting.Issuer,                    //使用者
                ValidAudience = jwtSetting.Audience,                //颁发者
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecreKey)) //加密方式
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SuperAdminOnly", policy => policy.RequireClaim("SuperAdminOnly"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            //services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped<IBaseAppService, BaseAppService>(pro =>   //假设我这里所有的接口，类分别继承自IBaseAppservice
            //{
            //    var dataAccess = Assembly.GetExecutingAssembly(); //拿到程序集
            //    Type[] types = dataAccess.GetTypes();           //找到约定的接口
            //    var checkTypes = types.Where(t => t.Name.Contains("IBaseAppService")).ToList();
            //    return (BaseAppService)pro.GetService(typeof(IBaseAppService));
            //});
            services.AddScoped<IDocumentService, DocumentService>();

            services.AddMvc(options =>
            {
                options.Filters.Add(new SampleGlobalActionFilter());
            });//.AddControllersAsServices()
               ////替换默认DI容器
               //var containerBuilder = new ContainerBuilder();
               //containerBuilder.RegisterModule<DefaultModule>();
               //////属性注入控制器、、、
               ////containerBuilder.RegisterType<AutoDIController>().PropertiesAutowired();
               //containerBuilder.Populate(services);
               ////containerBuilder.RegisterType<TaskFileController>().PropertiesAutowired();
               //var container = containerBuilder.Build();
               //return new AutofacServiceProvider(container);


            services.AddScoped<SampleControllerFilterAttribute>();
            services.AddScoped<SampleActionFilterAttribute>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp)
        {
            WorkingTask.Services.ServiceProvider.Provider = svp;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            
            app.UseAuthentication(); //加入认证中间件
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMvc(routes=>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Operations}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
