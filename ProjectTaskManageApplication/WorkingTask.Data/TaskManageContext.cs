using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WoringTask.Core.Data;
using WorkingTask.Data.BaseRepository;
using WorkingTask.Data.Models;

namespace WorkingTask.Data
{
    public class TaskManageContext: IdentityDbContext<TaskManageUser, TaskManageUserRole, string>, IUnitOfWork
    {
        public TaskManageContext(DbContextOptions<TaskManageContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                builder.ApplyConfiguration(configurationInstance); //.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(builder);
        }

        public virtual DbSet<WorkingTask.Data.Models.WorkingTask> WorkingTasks { get; set; }
        public virtual DbSet<DocumentInfo> DocumentInfos { get; set; }
        public virtual DbSet<WorkingTaskItems> WorkingTaskItems { get; set; }
        public virtual DbSet<WorkTaskSubmieOperations> WorkTaskSubmieOperations { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
