using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WoringTask.Core.DomainEntity
{
    public class OrderFactory
    {
        public virtual Order Create(ShoppingCart shoppingCart)
        {
            var order = new Order
            {
                Customer = shoppingCart.Customer,
                Items = shoppingCart.Items.Select(item => new OrderItem()
                {
                    Product = item.Product,
                    Quantity = item.Quantity
                }).ToList()
            };
            return order;
        }
    }
}
