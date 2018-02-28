using System;
using System.Collections.Generic;
using System.Text;

namespace WoringTask.Core.Data
{
    public partial interface IRepository<T>: IQuery<T> where T : BaseEntity<T>
    {
        bool Insert(T eneity);
        bool Update(T entity);
        bool Delete(T entity);
    }
}
