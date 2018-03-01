using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkingTask.Services.CustomerAppService;
using ProjectTaskManageApplication.ViewModel;

namespace ProjectTaskManageApplication.Controllers
{
    /// <summary>
    /// 测试注册用户
    /// </summary>
    [Produces("application/json")]
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerService"></param>
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _customerService.Register(registerViewModel.Email, registerViewModel.Name, registerViewModel.Password);
                if(result!=null)
                {
                    return Ok(new { result = result });
                }
                else
                {
                    return BadRequest(new { Code = "501", ErrorMessage = "角色创建失败" });
                }
            }
            else 
                return BadRequest(new { Code = "501", ErrorMessage = "信息填写不完整" });
        }
    }
}