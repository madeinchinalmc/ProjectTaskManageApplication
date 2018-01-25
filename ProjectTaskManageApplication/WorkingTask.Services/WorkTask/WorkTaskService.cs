using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkingTask.Data;
using WorkingTask.Data.Models;

namespace WorkingTask.Services.WorkTask
{
    public class WorkTaskService : IWorkTaskService
    {
        private readonly TaskManageContext _taskManageContext;
        public WorkTaskService()
        {
            _taskManageContext = DbContextFactory.GetCurrentDbContext() as TaskManageContext; 
        }
        public async Task<int> AddWrokTask(Data.Models.WorkingTask workingTask)
        {
            var result =await _taskManageContext.CustomAdd<Data.Models.WorkingTask>(workingTask);
            await _taskManageContext.CustomAdd<WorkTaskSubmieOperations>(
                new WorkTaskSubmieOperations
                {


                });
            return result.Id;
        }

        public async Task DelWrokTask(int Id)
        {
            var delEntity = await _taskManageContext.CustomExist<Data.Models.WorkingTask>(t=>t.Id == Id);
            if(delEntity)
            {
                await _taskManageContext.CustomDel(await _taskManageContext.FindAsync<Data.Models.WorkingTask>(new {
                    Id = Id
                }));
            }
        }

        public IList<Data.Models.WorkingTask> GetWrokTaskByPage(int pageIndex, int pageSize, out int totalCount, Expression<Func<Data.Models.WorkingTask, bool>> whereLambda, bool isAsc, Expression<Func<Data.Models.WorkingTask, int>> orderBy)
        {
            var listEntity = _taskManageContext.PageAsQuery<Data.Models.WorkingTask, int>(pageIndex, pageSize, out totalCount, whereLambda, isAsc, orderBy);
            return listEntity.ToList();
        }

        public async Task<Data.Models.WorkingTask> GetWrokTaskById(int Id)
        {
            var entity = await _taskManageContext.FindAsync<Data.Models.WorkingTask>(new
            {
                Id = Id
            });
            return entity;
        }

        public async Task UpWrokTask(Data.Models.WorkingTask workingTask)
        {
            var upEntity = await _taskManageContext.CustomExist<Data.Models.WorkingTask>(t => t.Id == workingTask.Id);
            if (upEntity)
            {
                await _taskManageContext.CustomUp(workingTask);
            }
        }
    }
}
