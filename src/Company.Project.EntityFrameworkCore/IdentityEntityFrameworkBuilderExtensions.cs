﻿using System;
using System.Reflection;
using Company.Project.Authorization.Roles;
using Company.Project.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Company.Project
{
    /// <summary>
    /// Contains extension methods to <see cref="IdentityBuilder"/> for adding entity framework stores.
    /// </summary>
    public static class IdentityEntityFrameworkBuilderExtensions
    {
        /// <summary>
        /// Adds an Entity Framework implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddAppEntityFrameworkStores<TContext>(this IdentityBuilder builder)
            where TContext : DbContext
        {
            AddStores(builder.Services, builder.UserType, builder.RoleType, typeof(TContext));
            return builder;
        }

        private static void AddStores(IServiceCollection services, Type userType, Type roleType, Type contextType)
        {
            var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
            if (identityUserType == null)
            {
                throw new InvalidOperationException("NotIdentityUser");
            }

            var keyType = identityUserType.GenericTypeArguments[0];

            if (roleType != null)
            {
                var identityRoleType = FindGenericBaseType(roleType, typeof(IdentityRole<>));
                if (identityRoleType == null)
                {
                    throw new InvalidOperationException("NotIdentityRole");
                }

                Type userStoreType = null;
                Type roleStoreType = null;
                var identityContext = FindGenericBaseType(contextType, typeof(IdentityDbContext<,,,,,,,>));
                if (identityContext == null)
                {
                    // If its a custom DbContext, we can only add the default POCOs
                    userStoreType = typeof(AppUserStore<,,,>).MakeGenericType(userType, roleType, contextType, keyType);
                    roleStoreType = typeof(AppRoleStore<,,>).MakeGenericType(roleType, contextType, keyType);
                }
                else
                {
                    userStoreType = typeof(AppUserStore<,,,,,,,,>).MakeGenericType(userType, roleType, contextType,
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[3],
                        identityContext.GenericTypeArguments[4],
                        identityContext.GenericTypeArguments[5],
                        identityContext.GenericTypeArguments[7],
                        identityContext.GenericTypeArguments[6]);
                    roleStoreType = typeof(AppRoleStore<,,,,>).MakeGenericType(roleType, contextType,
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[4],
                        identityContext.GenericTypeArguments[6]);
                }
                services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
                services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
            }
            else
            {   // No Roles
                Type userStoreType = null;
                var identityContext = FindGenericBaseType(contextType, typeof(IdentityUserContext<,,,,>));
                if (identityContext == null)
                {
                    // If its a custom DbContext, we can only add the default POCOs
                    userStoreType = typeof(AppUserOnlyStore<,,>).MakeGenericType(userType, contextType, keyType);
                }
                else
                {
                    userStoreType = typeof(AppUserOnlyStore<,,,,,>).MakeGenericType(userType, contextType,
                        identityContext.GenericTypeArguments[1],
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[3],
                        identityContext.GenericTypeArguments[4]);
                }
                services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
            }

        }

        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}