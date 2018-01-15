using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkingTask.Data.Models;

namespace WorkingTask.Data
{
    public class TaskManageContext: IdentityDbContext<TaskManageUser, TaskManageUserRole, string>
    {
        public TaskManageContext(DbContextOptions<TaskManageContext> options) : base(options) { }
        public virtual DbSet<WorkingTask.Data.Models.WorkingTask> WorkingTasks { get; set; }
        public virtual DbSet<DocumentInfo> DocumentInfos { get; set; }
        public virtual DbSet<WorkingTaskItems> WorkingTaskItems { get; set; }
        public virtual DbSet<WorkTaskSubmieOperations> WorkTaskSubmieOperations { get; set; }
    }
}
