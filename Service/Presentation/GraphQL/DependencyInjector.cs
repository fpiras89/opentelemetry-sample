using GraphQL.Conventions;
using System.Reflection;

namespace Examples.Service.Presentation.GraphQL
{
    internal class DependencyInjector : IDependencyInjector
    {
        private readonly IServiceProvider provider;

        public DependencyInjector(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public object Resolve(TypeInfo typeInfo) => provider.GetService(typeInfo);
    }
}
