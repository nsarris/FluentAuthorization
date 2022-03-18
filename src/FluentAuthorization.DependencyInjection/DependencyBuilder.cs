using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentAuthorization.DependencyInjection
{
    public class DependencyBuilder<TUser>
    {
        internal bool CustomPolicyContextProvider { get; set; }
        
        public IServiceCollection Services { get; }

        public DependencyBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Define your own PolicyContextProvider implementation and marker interface. Use this to encapsulate your TUser type to avoid repetion.
        /// </summary>
        /// <typeparam name="TService">The type of of PolicyContextProvider implementation.</typeparam>
        /// <typeparam name="TImplementation">The type of of PolicyContextProvider interface.</typeparam>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public DependencyBuilder<TUser> AddCustomPolicyContextProvider<TService, TImplementation>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TService : IPolicyContextProvider<TUser>
            where TImplementation : TService
        {
            Services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime));
            Services.Map<IPolicyContextProvider<TUser>, TService>();

            CustomPolicyContextProvider = true;
            
            return this;
        }

        /// <summary>
        /// Define your own PolicyContextProvider marker interface. Use this to encapsulate your TUser type to avoid repetion.
        /// </summary>
        /// <typeparam name="TService">The type of of PolicyContextProvider implementation.</typeparam>
        /// <typeparam name="TImplementation">The type of of PolicyContextProvider interface.</typeparam>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public DependencyBuilder<TUser> AddCustomPolicyContextProvider<TService>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TService : IPolicyContextProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(TService), typeof(PolicyContextProvider<TUser>), serviceLifetime));
            Services.Map<IPolicyContextProvider<TUser>, TService>();

            CustomPolicyContextProvider = true;

            return this;
        }

        /// <summary>
        /// Add a required user context provider dependency.
        /// </summary>
        public DependencyBuilder<TUser> AddUserContextProvider<TService, TImplementation>(ServiceLifetime serviceLifetime)
            where TService : IUserContextProvider<TUser>
            where TImplementation : TService
        {
            Services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime));
            Services.Map<IUserContextProvider<TUser>, TService>();
            return this;
        }

        /// <summary>
        /// Add a required user context provider dependency.
        /// </summary>
        public DependencyBuilder<TUser> AddUserContextProvider<TImplementation>(ServiceLifetime serviceLifetime)
            where TImplementation : IUserContextProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(TImplementation), serviceLifetime));
            Services.Map<IUserContextProvider<TUser>, TImplementation>();
            return this;
        }

        /// <summary>
        /// Add a required user context provider dependency.
        /// </summary>
        public DependencyBuilder<TUser> AddUserContextProvider<TImplementation>(TImplementation instance)
            where TImplementation : IUserContextProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(TImplementation), instance));
            Services.Map<IUserContextProvider<TUser>, TImplementation>();
            return this;
        }

        /// <summary>
        /// Add a required user context provider dependency.
        /// </summary>
        public DependencyBuilder<TUser> AddUserContextProvider<TImplementation>(Func<IServiceProvider, TImplementation> factory, ServiceLifetime serviceLifetime)
            where TImplementation : IUserContextProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(TImplementation), sp => factory(sp), serviceLifetime));
            Services.Map<IUserContextProvider<TUser>, TImplementation>();
            return this;
        }

        /// <summary>
        /// Add a required policy data provider dependency.
        /// </summary>
        public DependencyBuilder<TUser> AddDataProvider<TImplementation>(ServiceLifetime serviceLifetime)
            where TImplementation : IPolicyDataProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(IPolicyDataProvider<TUser>), typeof(TImplementation), serviceLifetime));
            return this;
        }

        /// <summary>
        /// Add a required policy data provider dependency.
        /// </summary>
        public DependencyBuilder<TUser> AddDataProvider<TImplementation>(TImplementation instance)
            where TImplementation : IPolicyDataProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(IPolicyDataProvider<TUser>), instance));
            return this;
        }

        /// <summary>
        /// Add a required policy data provider dependency.
        /// </summary>
        public DependencyBuilder<TUser> AddDataProvider<TImplementation>(Func<IServiceProvider, TImplementation> factory, ServiceLifetime serviceLifetime)
            where TImplementation : IPolicyDataProvider<TUser>
        {
            Services.Add(new ServiceDescriptor(typeof(IPolicyDataProvider<TUser>), sp => factory(sp), serviceLifetime));
            return this;
        }
    }
}

