using System;
using System.Collections.Generic;
using System.Text;
using WoringTask.Core.Data;
using System.Linq;

namespace WoringTask.Core.DomainEntity
{
    public class ShoppingCart : BaseEntity
    {
        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }
        #region Properties

        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<ShoppingCartItem> Items { get; set; }

        #endregion

        #region
        public void AddItem(Product product, int quantity)
        {
            var repetitiveCartItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if(repetitiveCartItem !=null)
            {
                repetitiveCartItem.Quantity += quantity;
                return;
            }
            Items.Add(new ShoppingCartItem
            {
                Product = product,
                ProductId = product.Id,
                Quantity = quantity
            });
        }
        public void ChangeProductQuantity(Guid productId,int newQuantity)
        {
            var items = Items as ICollection<ShoppingCartItem>;
            var existingCartItem = items.FirstOrDefault(i => i.ProductId == productId);
            if (existingCartItem == null)
                throw new InvalidOperationException("Cannot find the product in shopping cart");
            existingCartItem.Quantity = newQuantity;
        }
        public void RemoveItem(Guid productId)
        {
            var items = Items as ICollection<ShoppingCartItem>;

            var existingCartItem = items.FirstOrDefault(i => i.ProductId == productId);
            if (existingCartItem == null)
                throw new InvalidOperationException("Cannot find the product in shopping cart");
            items.Remove(existingCartItem);
        }
        #endregion
    }
}
