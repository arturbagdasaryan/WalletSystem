using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace WalletSystem.Api
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        private readonly ServiceProvider _provider;
        public DefaultDependencyResolver(ServiceProvider provider) => _provider = provider;
        public IDependencyScope BeginScope() => this;
        public object GetService(Type type) => _provider.GetService(type);
        public IEnumerable<object> GetServices(Type type) => _provider.GetServices(type);
        public void Dispose() => _provider.Dispose();
    }
}