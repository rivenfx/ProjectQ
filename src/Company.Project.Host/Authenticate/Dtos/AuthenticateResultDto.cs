namespace Company.Project.Authenticate.Dtos
{
    /// <summary>
    /// 认证结果
    /// </summary>
    public class AuthenticateResultDto
    {
        /// <summary>
        /// Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 加密之后的token
        /// </summary>
        public string EncryptedAccessToken { get; set; }

        /// <summary>
        /// 过期时间(秒)
        /// </summary>
        public int ExpireInSeconds { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 是否需要重置密码
        /// </summary>
        public bool ShouldResetPassword { get; set; }

        /// <summary>
        /// 密码重置code
        /// </summary>
        public string PasswordResetCode { get; set; }

        /// <summary>
        /// 返回
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 是否需要激活账号
        /// </summary>
        public bool WaitingForActivation { get; set; }
    }

}
