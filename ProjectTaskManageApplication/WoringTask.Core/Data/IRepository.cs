using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace WoringTask.Core.Data
{
    public partial interface IRepository<T>: IQuery<T> where T : BaseEntity
    {
        IEnumerable<T> Get(
            Expression<Func<T, Boolean>> predicate);
        bool Insert(T entity);
        bool Update(T entity);
        bool Delete(T entity);
    }
}
