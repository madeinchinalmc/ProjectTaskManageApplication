using System;
using WoringTask.Core.DomainEntity;
using WorkingTask.Data;
using WorkingTask.Services.BaseDomain;

namespace WorkingTask.Services.CustomerAppService
{
    public class CustomerService : BaseAppService, ICustomerService
    {
        private readonly WoringTask.Core.DomainEntity.CustomerService _DomainuUserService;
        public CustomerService(WoringTask.Core.DomainEntity.CustomerService DomainuUserService)
        {
            _DomainuUserService = DomainuUserService;
        }
        //protected WoringTask.Core.DomainEntity.CustomerService DomainuUserService
        //{
        //    get
        //    {
        //        return (WoringTask.Core.DomainEntity.CustomerService)MyHttpContext.Current.RequestServices.GetService(typeof(WoringTask.Core.DomainEntity.CustomerService));
        //    }
        //}
        public Customer Register(string email, string name, string password)
        {
            var customer = _DomainuUserService.Register(email, name, password);
            return customer;
        }
    }
}
