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
using WorkingTask.Services.TaskGroups;
using WorkingTask.Services.WorkTask;
using System.Runtime.CompilerServices;
using ProjectTaskManageApplication.Helper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using WorkingTask.Data.BaseRepository;
using WoringTask.Core.Data;

namespace ProjectTaskManageApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env; 
        }

        public IConfiguration Configuration { get; }

        private IHostingEnvironment Environment { get; }
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
            services.AddIdentity<TaskManageUser, TaskManageUserRole>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;     //和下面的设置密码规则一样的效果
            })
                .AddEntityFrameworkStores<TaskManageContext>()
                .AddDefaultTokenProviders();

            //var skipHTTPS = Configuration.GetValue<bool>("LocalTest:skipHTTPS");
            //services.Configure<MvcOptions>(options =>
            //{
            //    if (Environment.IsDevelopment() && !skipHTTPS)
            //    {
            //        options.Filters.Add(new RequireHttpsAttribute());
            //    }
            //});

            //设置注册密码的规则
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
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
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.RegisterAssembly("Service");
            //注册仓储
            services.AddScoped<WoringTask.Core.DomainEntity.CustomerService>();
            services.AddScoped<IRepository<BaseEntity> ,EfRepository<BaseEntity>>();
            ////注册工作单元
            //services.AddScoped<IUnitOfWork>(t => new TaskManageContext())
            services.Configure<MemoryCacheEntryOptions>(
                options => options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5));
            services.AddSession();
            services.AddMvc();
            //services.AddMvc(config =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //                        .RequireAuthenticatedUser()
            //                        .Build();
            //    config.Filters.Add(new AuthorizeFilter(policy));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp)
        {
            WorkingTask.Services.ServiceProvider.Provider = svp;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSession();
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
