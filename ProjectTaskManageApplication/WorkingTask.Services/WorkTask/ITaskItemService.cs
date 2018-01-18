using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WorkingTask.Services.WorkTask
{
    public interface ITaskItemService
    {
        WorkingTask.Data.Models.WorkingTaskItems GetWrokTaskById(int Id);

        IList<Data.Models.WorkingTaskItems> GetDocumentByPage(int workTaskId);

        int AddDocument(Data.Models.WorkingTaskItems taskItems);
        Task UpDocument(Data.Models.WorkingTaskItems taskItems);
        Task DelDocument(int Id);
    }
}
