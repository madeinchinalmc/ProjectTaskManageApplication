using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WorkingTask.Services.WorkTask
{
    public interface ITaskItemService
    {
        WorkingTask.Data.Models.WorkingTaskItems GetWrokTaskItemById(int Id);

        IList<Data.Models.WorkingTaskItems> GetTaskItemByTask(int workTaskId);

        Task<int> AddTaskItem(Data.Models.WorkingTaskItems taskItems);
        Task UpTaskItem(Data.Models.WorkingTaskItems taskItems);
        Task DelTaskItem(int Id);
    }
}
