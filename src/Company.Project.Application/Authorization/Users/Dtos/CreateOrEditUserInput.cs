using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Authorization.Users.Dtos
{
    public class CreateOrEditUserInput : UserEditDto
    {
        public string Password { get; set; }
    }
}
