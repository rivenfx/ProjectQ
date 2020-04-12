using Riven.Extensions;
using Riven.Uow;
using Riven.Dependency;

namespace Company.Project.Authorization
{
    public class AppIdentityStoreSessionAccessor : IAppIdentityStoreSessionAccessor, IScopeDependency
    {
        readonly IUnitOfWorkManager _unitOfWorkManager;

        public AppIdentityStoreSessionAccessor(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public TSession GetSession<TSession>()
            where TSession : class
        {
            return _unitOfWorkManager.Current.GetDbContext() as TSession;
        }
    }
}
