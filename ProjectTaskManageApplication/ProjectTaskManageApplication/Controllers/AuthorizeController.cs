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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace ProjectTaskManageApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizeController : Controller
    {
        private UserManager<TaskManageUser> _userManager; //加入Identity自带的注册使用的Manager
        private SignInManager<TaskManageUser> _signInManager; //加入Identity自带的登录使用的Manager
        private RoleManager<TaskManageUserRole> _roleManager;
        public JwtSettings _jwtSettings;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        public AuthorizeController(IOptions<JwtSettings> options
            , UserManager<TaskManageUser> userManager
            , SignInManager<TaskManageUser> signInManager
            , RoleManager<TaskManageUserRole> roleManager)
        {
            _jwtSettings = options.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [Route("api/token")]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)//[FromBody]
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _userManager.FindByNameAsync(loginViewModel.Name);
            if (result==null)
                return BadRequest();
            //验证密码
            var resultSigIn = await _signInManager.PasswordSignInAsync(loginViewModel.Name, loginViewModel.PassWord, true, false);
            if (!resultSigIn.Succeeded)
                return BadRequest();
            //获取角色
            var role = await _userManager.GetRolesAsync(result);
            //var claims = new Claim[]                //实例化一个Claim
            //{
            //    new Claim(ClaimTypes.Name,"lmc"),
            //    new Claim(ClaimTypes.Role, "admin"),
            //    new Claim("SuperAdminOnly","true")
            //};
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, result.UserName),
                new Claim(ClaimTypes.Role,role.Count==0?"":role[0])
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecreKey));  //将appsettings里面的SecreKey拿到
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);             //使用HmacSha256 算法加密
            //生成token，设置过期时间为30分钟， 需要引用System.IdentityModel.Tokens.Jwt 包
            var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, DateTime.Now, DateTime.Now.AddMinutes(30), creds);
            //将token返回
            TokenModel tokenModel = new TokenModel { token = new JwtSecurityTokenHandler().WriteToken(token) };
            return Ok(tokenModel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [Route("api/register")]
        [AllowAnonymous]
        [HttpPost]
        //[ValidateAntiForgeryToken]
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
                var roleResult = await SetUserRoleDefault(taskManageUser);
                if (roleResult.Succeeded)
                {
                    var token = await this.Index(loginViewModel);
                    return token;
                }
                else
                {
                    return BadRequest(new { Code = "501", ErrorMessage = "用户创建角色失败" });
                }
            }
            else
            {
                return BadRequest(new { Code = "501", ErrorMessage = "用户创建失败" });
            }
        }
        /// <summary>
        /// 登出，api不需要
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }
        private async Task<IdentityResult> SetUserRoleDefault(TaskManageUser taskManageUser)
        {
            var role = await _roleManager.FindByNameAsync("tourist");
            await _userManager.AddToRoleAsync(taskManageUser, role.Name);
            var result = await _userManager.UpdateAsync(taskManageUser);
            return result;
        }
        /// <summary>
        /// 创建一个角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [Route("api/addrole")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var result = await _roleManager.FindByNameAsync(roleName);
            if(result!=null)
                return BadRequest(new { Code = "501", ErrorMessage = "角色已存在" });
            var roleResult = await _roleManager.CreateAsync(new TaskManageUserRole(roleName));
            if (roleResult.Succeeded)
                return Ok(new { result = roleResult });
            else
                return BadRequest(new { Code = "501", ErrorMessage = "角色创建失败" });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [Route("api/addRoleforUser")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UpRoleForUser(string UserName,string roleName)
        {
            var resultRole = await _roleManager.FindByNameAsync(roleName);
            if (resultRole == null)
                return BadRequest(new { Code = "501", ErrorMessage = "角色不存在" });
            var resultUser = await _userManager.FindByNameAsync(UserName);
            if (resultUser == null)
                return BadRequest(new { Code = "501", ErrorMessage = "用户不存在" });
            await _userManager.AddToRoleAsync(resultUser, resultRole.Name);
            var result = await _userManager.UpdateAsync(resultUser);
            if (result.Succeeded)
                return Ok(new { result = result,Code ="200", Message ="授权成功"});
            else
                return BadRequest(new { Code = "501", ErrorMessage = "授权失败" });
        }
    }
}