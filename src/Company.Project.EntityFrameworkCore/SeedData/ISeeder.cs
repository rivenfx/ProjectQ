using Riven.Repositories;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Company.Project.SeedData
{
    public interface ISeeder
    {
        Task Create();
    }
}
