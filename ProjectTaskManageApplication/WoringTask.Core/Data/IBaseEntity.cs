using System;
using System.Collections.Generic;
using System.Text;

namespace WoringTask.Core.Data
{
    public interface IBaseEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
        bool IsTransient();
    }
}
