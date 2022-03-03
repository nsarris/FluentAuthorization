using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public class DependencyBuilder<TUser>
    {
        internal bool CustomPolicyContextProvider { get; set; }
        
        public IServiceCollection Services { get; }

        public DependencyBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public DependencyBuilder<TUser> AddCustomPolicyContextProvider<TService, TImplementation>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TService : IPolicyContextProvider<TUser>
            where TImplementation : TService
        {
            Services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime));
            Services.Map<IPolicyContextProvider<TUser>, TService>();

            CustomPolicyContextProvider = true;
            
            return this;
        }


        public DependencyBuilder<TUser> AddUserContextProvider<TService, TImplementation>(ServiceLifetime serviceLifetime)
            where TService : IUserContextProvider<TUser>
            where TImplementation : TService
        {
            Services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime));
            Services.Map<IUserContextProvider<TUser>, TService>();
            return this;
        }

        public DependencyBuilder<TUser> AddUserContextProvider<TImplementation>(ServiceLifetime serviceLifetime)
            where TImplementation : IUserContextProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(TImplementation), serviceLifetime));
            Services.Map<IUserContextProvider<TUser>, TImplementation>();
            return this;
        }

        public DependencyBuilder<TUser> AddUserContextProvider<TImplementation>(TImplementation instance)
            where TImplementation : IUserContextProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(TImplementation), instance));
            Services.Map<IUserContextProvider<TUser>, TImplementation>();
            return this;
        }

        public DependencyBuilder<TUser> AddUserContextProvider<TImplementation>(Func<IServiceProvider, TImplementation> factory, ServiceLifetime serviceLifetime)
            where TImplementation : IUserContextProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(TImplementation), sp => factory(sp), serviceLifetime));
            Services.Map<IUserContextProvider<TUser>, TImplementation>();
            return this;
        }



        //public DependencyBuilder<TUser> AddCustomPolicyProvider<TService, TImplementation>(ServiceLifetime serviceLifetime)
        //    where TService : IPolicyProvider
        //    where TImplementation : TService
        //{
        //    Services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime));
        //    CustomPolicyProvider = true;
        //    return this;
        //}

        //public DependencyBuilder<TUser> AddCustomPolicyProvider<TImplementation>(ServiceLifetime serviceLifetime)
        //    where TImplementation : IPolicyProvider
        //{
        //    Services.Add(new ServiceDescriptor(typeof(IPolicyProvider), typeof(TImplementation), serviceLifetime));
        //    CustomPolicyProvider = true;
        //    return this;
        //}

        //public DependencyBuilder<TUser> AddCustomPolicyProvider<TImplementation>(TImplementation instance)
        //    where TImplementation : IPolicyProvider
        //{
        //    Services.Add(new ServiceDescriptor(typeof(IPolicyProvider), instance));
        //    CustomPolicyProvider = true;
        //    return this;
        //}

        //public DependencyBuilder<TUser> AddCustomPolicyProvider<TImplementation>(Func<IServiceProvider, TImplementation> factory, ServiceLifetime serviceLifetime)
        //    where TImplementation : IPolicyProvider
        //{
        //    Services.Add(new ServiceDescriptor(typeof(IPolicyProvider), sp => factory(sp), serviceLifetime));
        //    CustomPolicyProvider = true;
        //    return this;
        //}

        //public DependencyBuilder<TUser> AddDefaultPolicyProvider()
        //{
        //    Services.AddTransient<IPolicyProvider, DefaultPolicyProvider>();
        //    return this;
        //}



        //public DependencyBuilder<TUser> AddDataProvider<TService, TImplementation>(ServiceLifetime serviceLifetime)
        //    where TService : IPolicyDataProvider<TUser>
        //    where TImplementation : TService
        //{
        //    Services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime));
        //    return this;
        //}

        public DependencyBuilder<TUser> AddDataProvider<TImplementation>(ServiceLifetime serviceLifetime)
            where TImplementation : IPolicyDataProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(IPolicyDataProvider<TUser>), typeof(TImplementation), serviceLifetime));
            return this;
        }

        public DependencyBuilder<TUser> AddDataProvider<TImplementation>(TImplementation instance)
            where TImplementation : IPolicyDataProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(IPolicyDataProvider<TUser>), instance));
            return this;
        }

        public DependencyBuilder<TUser> AddDataProvider<TImplementation>(Func<IServiceProvider, TImplementation> factory, ServiceLifetime serviceLifetime)
            where TImplementation : IPolicyDataProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(IPolicyDataProvider<TUser>), sp => factory(sp), serviceLifetime));
            return this;
        }
    }
}

