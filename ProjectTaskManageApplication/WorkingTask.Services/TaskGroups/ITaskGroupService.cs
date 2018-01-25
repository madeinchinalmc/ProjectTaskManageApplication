using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkingTask.Data.Models;

namespace WorkingTask.Services.TaskGroups
{
    public interface ITaskGroupService
    {
        Task<int> AddNewTaskGroup(TaskGroup taskGroup);

        Task UpTaskGroup(TaskGroup taskGroup);

        Task DelTaskGroup(int Id);
        /// <summary>
        /// 根据任务查询组内成员
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        IList<TaskGroup> QueryTaskGroup(int TaskId);
    }
}
