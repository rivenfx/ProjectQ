namespace Company.Project.Configuration
{
    /// <summary>
    /// jwt认证配置
    /// </summary>
    public class JwtBearerInfo
    {
        public string Audience { get; set; }

        public string Authority { get; set; }
    }
}
