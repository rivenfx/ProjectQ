using Company.Project.Authorization.Users;
using Company.Project.Authorization.Users.Dtos;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.ObjectMapper
{
    /// <summary>
    /// Application模块的对象映射器
    /// </summary>
    public class ApplicationModuleObjectMapperRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<User, UserEditDto>()
            //    .TwoWays() // 双向
            //    .Map((dest) => dest.UserName, (src) => src.UserName ?? string.Empty); // 指定字段映射


        }
    }
}
