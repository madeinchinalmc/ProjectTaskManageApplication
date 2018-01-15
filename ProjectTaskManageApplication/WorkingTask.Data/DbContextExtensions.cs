using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WorkingTask.Data
{
    public static class DbContextExtensions
    {
        public static IQueryable<T> PageAsQuery<T, TKey>(this DbContext dbContext, int pageIndex, int pageSize,out int totalCount,
            Expression<Func<T,bool>> whereLambda,bool isAsc,Expression<Func<T,TKey>> orderBy) where T : class, new()
        {
            IQueryable<T> temp = dbContext.Set<T>().Where(whereLambda);
            totalCount = temp.Count();
            pageSize = Math.Min(pageSize, 20);
            if(isAsc)
            {
                temp = temp.OrderBy(orderBy)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending(orderBy)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize);
            }
            return temp.AsNoTracking();// 不要跟踪，提高查询性能
        }
        public static async Task<bool> Exist<T>(this DbContext dbContext, Expression<Func<T, bool>> anyLambda) where T : class, new()
        {
            return await dbContext.Set<T>().AnyAsync(anyLambda);
        }
        public static async Task<T> Find<T>(this DbContext dbContext, params object[] keyValues) where T : class, new()
        {
            return await dbContext.Set<T>().FindAsync(keyValues);
        }
        public static async Task<int> Count<T>(this DbContext dbContext, Expression<Func<T, bool>> countLambda) where T : class, new()
        {
            return await dbContext.Set<T>().CountAsync(countLambda);
        }
        public static async Task<T> First<T>(this DbContext dbContext, Expression<Func<T, bool>> firstLambda) where T : class, new()
        {
            return await dbContext.Set<T>().FirstOrDefaultAsync(firstLambda);
        }
        public static async Task<int> SaveChanges(this DbContext dbContext)
        {
            try
            {
                return await dbContext.SaveChangesAsync();
            }

            catch (DbUpdateException ex)
            {

                //Common.LogHelper.WriteLog(this.GetType(), ex.InnerException == null ? ex : ex.InnerException.InnerException);
                throw;
            }
            catch (Exception ex)
            {
                //Common.LogHelper.WriteLog(this.GetType(), ex);
                throw;
            }

        }
    }
}
