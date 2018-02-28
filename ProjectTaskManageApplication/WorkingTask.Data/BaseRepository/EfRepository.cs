using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WoringTask.Core.Data;

namespace WorkingTask.Data.BaseRepository
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity<T>
    {
        public IQueryable<T> Table => throw new NotImplementedException();

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public T GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Insert(T eneity)
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
