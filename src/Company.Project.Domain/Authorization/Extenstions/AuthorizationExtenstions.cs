using JetBrains.Annotations;
using Riven;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Company.Project.Authorization.Extenstions
{
    public static class AuthorizationExtenstions
    {
        public static Claim ToClaim([NotNull] this string claim)
        {
            Check.NotNullOrWhiteSpace(claim, nameof(claim));

            return new Claim(claim, claim);
        }

        public static IEnumerable<Claim> ToClaims([NotNull]this string[] claims)
        {
            Check.NotNull(claims, nameof(claims));

            foreach (var claim in claims)
            {
                yield return claim.ToClaim();
            }
        }

        public static IEnumerable<Claim> ToClaims([NotNull]this IEnumerable<string> claims)
        {
            Check.NotNull(claims, nameof(claims));

            foreach (var claim in claims)
            {
                yield return claim.ToClaim();
            }
        }
    }
}
