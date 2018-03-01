using System;
using System.Collections.Generic;
using System.Text;
using WoringTask.Core.Data;

namespace WoringTask.Core.DomainEntity
{
    public class OrderItem : BaseEntity
    {
        public System.Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
