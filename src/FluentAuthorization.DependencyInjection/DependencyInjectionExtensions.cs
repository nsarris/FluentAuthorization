﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentAuthorization.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddFluentAuthorization<TUser>(this IServiceCollection services, Action<DependencyBuilder<TUser>> configure)
        {
            var builder = new DependencyBuilder<TUser>(services);
            configure(builder);

            if (!builder.CustomPolicyContextProvider)
            {
                services.AddTransient<IPolicyContextProvider<TUser>, PolicyContextProvider<TUser>>();
            }

            return services;
        }

        internal static IServiceCollection Map(this IServiceCollection services, Type source, Type target)
        {
            services.AddTransient(source, sp => sp.GetRequiredService(target));
            return services;
        }

        internal static IServiceCollection Map<TSource, TTarget>(this IServiceCollection services) 
            => services.Map(typeof(TSource), typeof(TTarget));
    }
}

