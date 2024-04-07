using GraphQL.Conventions;
using System.Reflection;

namespace Examples.Service.Presentation.GraphQL
{
    internal sealed class Injector : IDependencyInjector
    {
        private readonly IServiceProvider provider;

        public Injector(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public object Resolve(TypeInfo typeInfo) => provider.GetService(typeInfo);
    }
}
