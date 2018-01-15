using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkingTask.Data;

namespace WorkingTask.Services
{
    /// <summary>
    /// 数据库上下文附加到http上下文
    /// </summary>
    public class DbContextFactory
    {
        public static DbContext GetCurrentDbContext()
        {
            var httpContext = MyHttpContext.Current;
            var context = (DbContext)httpContext.RequestServices.GetService(typeof(TaskManageContext));
            return context;
        }
    }
}
