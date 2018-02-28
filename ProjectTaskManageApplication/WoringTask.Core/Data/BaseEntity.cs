using System;
using System.Collections.Generic;
using System.Text;

namespace WoringTask.Core.Data
{
    public class BaseEntity<TPrimaryKey> : IBaseEntity<TPrimaryKey>
    {
        public BaseEntity() { }

        public virtual TPrimaryKey Id { get; set; }

        public virtual bool IsTransient()
        {
            throw new NotImplementedException();
        }
    }
}
