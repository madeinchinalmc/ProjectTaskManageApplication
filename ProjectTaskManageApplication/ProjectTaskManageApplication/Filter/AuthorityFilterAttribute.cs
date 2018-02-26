using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTaskManageApplication.Filter
{
    /// <summary>
    /// 检验权限和登录的filter
    /// </summary>
    public class AuthorityFilterAttribute: AuthorizeAttribute
    {
        private string _LoginPath = "";
        public AuthorityFilterAttribute()
        {
            this._LoginPath = "/Fourth/Login";
        }
    }
}
