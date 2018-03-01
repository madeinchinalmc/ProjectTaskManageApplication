using System;
using System.Collections.Generic;
using System.Text;

namespace WoringTask.Core.Data
{
    public interface IUnitOfWork:IDisposable
    {
        int SaveChanges();
    }
}
