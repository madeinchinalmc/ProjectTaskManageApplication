using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkingTask.Data;
using WorkingTask.Data.Models;

namespace WorkingTask.Services.WorkTask
{
    public class TaskItemService : ITaskItemService
    {
        private readonly TaskManageContext _taskManageContext;

        public TaskItemService()
        {
            _taskManageContext = DbContextFactory.GetCurrentDbContext() as TaskManageContext;
        }
        public async Task<int> AddTaskItem(WorkingTaskItems taskItems)
        {
            var result = await _taskManageContext.CustomAdd<WorkingTaskItems>(taskItems);
            return result.Id;
        }

        public async Task DelTaskItem(int Id)
        {
            var delEntity = await _taskManageContext.CustomExist<Data.Models.WorkingTaskItems>(t => t.Id == Id);
            if (delEntity)
            {
                await _taskManageContext.CustomDel(await _taskManageContext.FindAsync<Data.Models.WorkingTaskItems>(new
                {
                    Id = Id
                }));
            }
        }

        public IList<WorkingTaskItems> GetTaskItemByTask(int workTaskId)
        {
            var entityList = _taskManageContext.QueryList<WorkingTaskItems>(t => t.WorkingTaskId == workTaskId);
            return entityList.ToList();
        }

        public WorkingTaskItems GetWrokTaskItemById(int Id)
        {
            var entity = _taskManageContext.Find<WorkingTaskItems>(new
            {
                Id = Id
            });
            return entity;
        }

        public async Task UpTaskItem(WorkingTaskItems taskItems)
        {
            if (await _taskManageContext.CustomExist<WorkingTaskItems>(t => t.Id == taskItems.Id))
            {
                await _taskManageContext.CustomUp<WorkingTaskItems>(taskItems);
            }
        }
    }
}
