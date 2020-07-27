using System.Collections.Generic;

namespace Company.Project.Session.Dtos
{
    /// <summary>
    /// Claims
    /// </summary>
    public class ClaimsDto
    {
        /// <summary>
        /// 所有的claims
        /// </summary>
        public List<string> AllClaims { get; set; }

        /// <summary>
        /// 当前登录用户拥有的claims
        /// </summary>
        public List<string> GrantedClaims { get; set; }
    }
}
