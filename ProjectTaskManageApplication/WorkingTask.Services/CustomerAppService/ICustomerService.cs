using System;
using System.Collections.Generic;
using System.Text;
using WorkingTask.Services.BaseDomain;
using WoringTask.Core.DomainEntity;


namespace WorkingTask.Services.CustomerAppService
{
    public interface ICustomerService: IBaseAppService
    {
       Customer Register(string email, string name, string password);
    }
}
