using System;
using System.Collections.Generic;
using System.Text;

namespace WorkingTask.Data.BaseRepository
{
    public interface IUnitOfWork
    {
        int SaveChanges();
    }
}
