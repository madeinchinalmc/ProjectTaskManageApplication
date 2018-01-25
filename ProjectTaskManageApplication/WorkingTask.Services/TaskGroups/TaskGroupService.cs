using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkingTask.Data;
using WorkingTask.Data.Models;

namespace WorkingTask.Services.TaskGroups
{
    public class TaskGroupService : ITaskGroupService
    {
        private readonly TaskManageContext _taskManageContext;

        public TaskGroupService()
        {
            _taskManageContext = DbContextFactory.GetCurrentDbContext() as TaskManageContext;
        }
        public async Task<int> AddNewTaskGroup(TaskGroup taskGroup)
        {
            var result = await _taskManageContext.CustomAdd<TaskGroup>(taskGroup);
            return result.Id;
        }

        public async Task DelTaskGroup(int Id)
        {
            var delEntity = await _taskManageContext.CustomExist<Data.Models.TaskGroup>(t => t.Id == Id);
            if(delEntity)
                await _taskManageContext.CustomDel(await _taskManageContext.FindAsync<Data.Models.WorkingTask>(new
                {
                    Id = Id
                }));
        }

        public IList<TaskGroup> QueryTaskGroup(int TaskId)
        {
            var entity =  _taskManageContext.QueryList<TaskGroup>(t=>t.WorkingTaskId == TaskId);
            return entity.ToList();
        }

        public async Task UpTaskGroup(TaskGroup taskGroup)
        {
            var upEntity = await _taskManageContext.CustomExist<Data.Models.TaskGroup>(t => t.Id == taskGroup.Id);
            if (upEntity)
            {
                await _taskManageContext.CustomUp(taskGroup);
            }
        }
    }
}
