using Company.Project.Authorization.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Riven.Uow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Authorization
{
    public class AppSecurityStampValidator : SecurityStampValidator<User>
    {
        readonly IUnitOfWorkManager _unitOfWorkManager;

        public AppSecurityStampValidator(
            IOptions<SecurityStampValidatorOptions> options,
            SignInManager signInManager,
            ISystemClock clock,
            ILoggerFactory logger,
            IUnitOfWorkManager unitOfWorkManager
            )
            : base(options, signInManager, clock, logger)
        {
            this._unitOfWorkManager = unitOfWorkManager;
        }

        public override async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var currentUtc = DateTimeOffset.UtcNow;
            if (context.Options != null && Clock != null)
            {
                currentUtc = Clock.UtcNow;
            }
            var issuedUtc = context.Properties.IssuedUtc;

            // Only validate if enough time has elapsed
            var validate = (issuedUtc == null);
            if (issuedUtc != null)
            {
                var timeElapsed = currentUtc.Subtract(issuedUtc.Value);
                validate = timeElapsed > Options.ValidationInterval;
            }

            // 需要刷新验证
            if (validate)
            {
                try
                {
                    // 启动工作单元
                    using (var uow = this._unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                    {
                        var user = await VerifySecurityStamp(context.Principal);
                        if (user != null)
                        {
                            await SecurityStampVerified(user, context);
                        }
                        else
                        {
                            Logger.LogDebug(0, "Security stamp validation failed, rejecting cookie.");
                            context.RejectPrincipal();
                            await SignInManager.SignOutAsync();
                        }

                        await uow.CompleteAsync(context.HttpContext.RequestAborted);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }
    }
}
