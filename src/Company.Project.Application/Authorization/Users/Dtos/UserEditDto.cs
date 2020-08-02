using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Users.Dtos
{
    public class UserEditDto
    {
        public UserDto EntityDto { get; set; }

        public List<Guid> Roles { get; set; }
    }
}
