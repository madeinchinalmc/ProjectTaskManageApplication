using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WoringTask.Core.Data;
using System.Linq.Expressions;

namespace WorkingTask.Data.BaseRepository
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private DbContext _context;

        public EfRepository(IUnitOfWork uow)
        {
            if (uow == null)
                throw new ArgumentNullException("uow");
            _context = uow as DbContext;
        }
        public IQueryable<T> Table
        {
            get { return _context.Set<T>().Where(t => true); }
        }


        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }

        public T GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public bool Insert(T entity)
        {
            _context.Set<T>().Add(entity);
            return true;
        }
        public bool Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry<T>(entity).State = EntityState.Modified;
            return true;
        }
        public bool Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return true;
        }
    }
}
