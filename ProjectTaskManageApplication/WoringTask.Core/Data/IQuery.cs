using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoringTask.Core.Data
{
    public interface IQuery<T>
    {
        T GetById(Guid id);
        IQueryable<T> Table { get; }
    }
}
