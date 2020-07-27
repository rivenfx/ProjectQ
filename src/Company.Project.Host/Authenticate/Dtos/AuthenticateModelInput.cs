using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Project.Authenticate.Dtos
{
    /// <summary>
    /// 认证输入
    /// </summary>
    public class AuthenticateModelInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// 记住客户端
        /// </summary>
        public bool RememberClient { get; set; }

        /// <summary>
        /// 返回的url地址
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 启用 cookie
        /// </summary>
        public bool UseCookie { get; set; } = true;

        /// <summary>
        /// 启用 token
        /// </summary>
        public bool UseToken { get; set; } = true;
    }

}
