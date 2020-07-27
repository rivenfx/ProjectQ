namespace Company.Project.Configuration
{
    /// <summary>
    /// jwt认证配置
    /// </summary>
    public class JwtBearerInfo
    {
        public JwtBearerInfo(string audience, string authority, string issuer, string securityKey)
        {
            Audience = audience;
            Authority = authority;
            Issuer = issuer;
            SecurityKey = securityKey;
        }

        public string Audience { get; }

        public string Authority { get; }

        public string Issuer { get; }

        public string SecurityKey { get; }
    }
}
