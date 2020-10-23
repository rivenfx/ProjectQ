using System.Collections.Generic;

namespace Company.Project.Session.Dtos
{
    /// <summary>
    /// Permission
    /// </summary>
    public class AuthDto
    {
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
