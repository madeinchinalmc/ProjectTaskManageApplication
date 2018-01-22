using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkingTask.Data;
using WorkingTask.Data.Models;

namespace WorkingTask.Services.WorkTask
{
    public interface IWorkTaskService
    {
        Task<Data.Models.WorkingTask> GetWrokTaskById(int Id);

        IList<Data.Models.WorkingTask> GetWrokTaskByPage(int pageIndex, int pageSize, out int totalCount,
            Expression<Func<Data.Models.WorkingTask, bool>> whereLambda, bool isAsc, Expression<Func<Data.Models.WorkingTask, int>> orderBy);

        Task<int> AddWrokTask(Data.Models.WorkingTask workingTask);
        Task UpWrokTask(Data.Models.WorkingTask workingTask);
        Task DelWrokTask(int Id);
    }
}
