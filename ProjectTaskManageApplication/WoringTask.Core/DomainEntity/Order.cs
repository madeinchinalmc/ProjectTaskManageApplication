using System;
using System.Collections.Generic;
using System.Text;
using WoringTask.Core.Data;

namespace WoringTask.Core.DomainEntity
{
    public class Order:BaseEntity
    {
        public virtual ICollection<OrderItem> Items { get; set; }
        public virtual Customer Customer { get; set; }
        public Guid CustomerId { get; set; }
    }
}
