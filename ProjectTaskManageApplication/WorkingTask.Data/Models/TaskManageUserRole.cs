using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkingTask.Data.Models
{
    public class TaskManageUserRole: IdentityRole
    {
        public TaskManageUserRole() { }
        public TaskManageUserRole(string RoleName):base(RoleName) { }
    }
}
