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
    }
}
