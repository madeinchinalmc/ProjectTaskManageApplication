using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskManageApplication.Models;
using Microsoft.Extensions.Options;
using ProjectTaskManageApplication.ViewModel;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using WorkingTask.Data.Models;

namespace ProjectTaskManageApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizeController : Controller
    {
        private UserManager<TaskManageUser> _userManager; //加入Identity自带的注册使用的Manager
        private SignInManager<TaskManageUser> _signInManager; //加入Identity自带的登录使用的Manager
        public JwtSettings _jwtSettings;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public AuthorizeController(IOptions<JwtSettings> options, UserManager<TaskManageUser> userManager, SignInManager<TaskManageUser> signInManager)
        {
            _jwtSettings = options.Value;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [Route("api/token")]
        [HttpPost]
        public async Task<IActionResult> Index([FromBody]LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _userManager.FindByNameAsync(loginViewModel.Name);
            if (result==null)
                return BadRequest();
            var resultSigIn = await _signInManager.PasswordSignInAsync(loginViewModel.Name, loginViewModel.PassWord, true, false);
            if (!resultSigIn.Succeeded)
                return BadRequest();
            var claims = new Claim[]                //实例化一个Claim
            {
                new Claim(ClaimTypes.Name,"lmc"),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim("SuperAdminOnly","true")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecreKey));  //将appsettings里面的SecreKey拿到
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);             //使用HmacSha256 算法加密
            //生成token，设置过期时间为30分钟， 需要引用System.IdentityModel.Tokens.Jwt 包
            var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, DateTime.Now, DateTime.Now.AddMinutes(30), creds);
            //将token返回

            return Json(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [Route("api/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]LoginViewModel loginViewModel)
        {
            var result = await _userManager.FindByNameAsync(loginViewModel.Name);
            if(result!= null)
            {
                return BadRequest(new { Code = "501", ErrorMessage = "该用户已存在" });
            }
            TaskManageUser taskManageUser = new TaskManageUser
            {
                Email = loginViewModel.Name,
                UserName = loginViewModel.Name,
                NormalizedUserName = loginViewModel.Name
            };
            var createResult = await _userManager.CreateAsync(taskManageUser, loginViewModel.PassWord);
            if (createResult.Succeeded)
            {
                var token = await this.Index(loginViewModel);
                return token;
            }
            else
            {
                return BadRequest(new { Code = "501", ErrorMessage = "用户创建失败" });
            }
        }

    }
}