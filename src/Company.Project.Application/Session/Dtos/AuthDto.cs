using System.Collections.Generic;

namespace Company.Project.Session.Dtos
{
    /// <summary>
    /// Permission
    /// </summary>
    public class AuthDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserNickName { get; set; }

        /// <summary>
        /// 所有的 Permission
        /// </summary>
        public List<string> AllPermissions { get; set; }

        /// <summary>
        /// 当前登录用户拥有的 Permission
        /// </summary>
        public List<string> GrantedPermissions { get; set; }
    }
}
