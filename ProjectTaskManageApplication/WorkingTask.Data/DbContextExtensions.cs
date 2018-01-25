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
        /// <summary>
        /// 查询返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public static IQueryable<T> QueryList<T>(this DbContext dbContext, Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            IQueryable<T> temp = dbContext.Set<T>().Where(whereLambda);
            return temp.AsNoTracking();
        }
        public static async Task<bool> CustomExist<T>(this DbContext dbContext, Expression<Func<T, bool>> anyLambda) where T : class, new()
        {
            return await dbContext.Set<T>().AnyAsync(anyLambda);
        }
        public static async Task<T> CustomFind<T>(this DbContext dbContext, params object[] keyValues) where T : class, new()
        {
            return await dbContext.Set<T>().FindAsync(keyValues);
        }
        public static async Task<int> CustomCount<T>(this DbContext dbContext, Expression<Func<T, bool>> countLambda) where T : class, new()
        {
            return await dbContext.Set<T>().CountAsync(countLambda);
        }
        public static async Task<T> CustomFirst<T>(this DbContext dbContext, Expression<Func<T, bool>> firstLambda) where T : class, new()
        {
            return await dbContext.Set<T>().FirstOrDefaultAsync(firstLambda);
        }
        public static async Task<T> CustomAdd<T>(this DbContext dbContext,T t) where T : class, new()
        {
            var result = await dbContext.Set<T>().AddAsync(t);
            await SaveChanges(dbContext);
            return result.Entity;
        }
        public static async Task CustomDel<T>(this DbContext dbContext,T t) where T : class, new()
        {
            var result = dbContext.Set<T>().Remove(t);
            await SaveChanges(dbContext);
        }
        public static async Task CustomUp<T>(this DbContext dbContext, T t) where T : class, new()
        {
            dbContext.Set<T>().Update(t);
            await SaveChanges(dbContext);
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task ExecuteInsertOnTransaction<T1,T2>(this DbContext dbContext,T1 t1,T2 t2) 
            where T1 : class, new()
            where T2: class, new()
        {
            using(var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                     dbContext.Set<T1>().Add(t1);
                     dbContext.Set<T2>().Add(t2);
                     await SaveChanges(dbContext);
                     transaction.Commit();
                }
                catch(Exception ex)
                {
                    //log
                }
            }
        }
    }
}
