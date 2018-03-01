using System;
using System.Collections.Generic;
using System.Text;
using WoringTask.Core.Data;

namespace WoringTask.Core.DomainEntity
{
    public class CustomerService
    {
        private IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public virtual Customer Register(string email, string name, string password)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Email = email,
                Name = name,
                Password = password
            };
            customer.CreateShoppingCart();
            _customerRepository.Insert(customer);
            return customer;
        }
    }
}
