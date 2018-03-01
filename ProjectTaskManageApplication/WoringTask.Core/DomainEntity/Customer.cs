using System;
using System.Collections.Generic;
using System.Text;
using WoringTask.Core.Data;

namespace WoringTask.Core.DomainEntity
{
    public class Customer:BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public void CreateShoppingCart()
        {
            ShoppingCart = new ShoppingCart
            {
                Id = Guid.NewGuid(),
                Customer = this,
                CustomerId = Id,
            };

            ShoppingCartId = ShoppingCart.Id;
        }
    }
}
