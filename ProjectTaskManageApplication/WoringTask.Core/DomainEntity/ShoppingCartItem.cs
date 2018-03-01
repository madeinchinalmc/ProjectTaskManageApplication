using System;
using System.Collections.Generic;
using System.Text;
using WoringTask.Core.Data;

namespace WoringTask.Core.DomainEntity
{
    public class ShoppingCartItem:BaseEntity
    {
        public Guid CartId { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
