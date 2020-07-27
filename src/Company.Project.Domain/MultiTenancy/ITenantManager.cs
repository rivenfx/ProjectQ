using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Riven.Dependency;
using Riven.Repositories;

namespace Company.Project.MultiTenancy
{
    public interface ITenantManager : ITransientDependency
    {
        IQueryable<Tenant> Query { get; }
        IQueryable<Tenant> QueryAsNoTracking { get; }

        /// <summary>
        /// �����⻧
        /// </summary>
        /// <param name="name">����(Ψһ)</param>
        /// <param name="displayName">��ʾ����</param>
        /// <param name="description">����</param>
        /// <param name="connectionString">���ݿ������ַ���</param>
        /// <param name="isStatic">�Ƿ�Ϊϵͳ����</param>
        /// <returns></returns>
        Task<Tenant> Create(string name, string displayName, string description = null, string connectionString = null, bool isStatic = false);

        /// <summary>
        /// �����⻧���ƻ�ȡ�⻧
        /// </summary>
        /// <param name="name">����</param>
        /// <returns></returns>
        Task<Tenant> GetByName(string name);

        /// <summary>
        /// �����⻧
        /// </summary>
        /// <param name="name">�⻧����</param>
        /// <param name="displayName">��ʾ����</param>
        /// <param name="description">��ϸ</param>
        /// <returns></returns>
        Task<Tenant> Update(string name, string displayName, string description);

        /// <summary>
        /// ��������ɾ���⻧
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task Delete(string name);
    }
}
