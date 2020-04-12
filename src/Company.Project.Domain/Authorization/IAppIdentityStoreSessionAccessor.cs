using Riven.Uow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Company.Project.Authorization
{
    /// <summary>
    /// 应用程序 Identity Store Session 访问者
    /// </summary>
    public interface IAppIdentityStoreSessionAccessor
    {
        /// <summary>
        /// 获取 Store Session
        /// </summary>
        /// <typeparam name="TSession"></typeparam>
        /// <returns></returns>
        [UnitOfWork]
        TSession GetSession<TSession>()
            where TSession : class;
    }
}
