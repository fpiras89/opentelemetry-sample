using GraphQL;
using GraphQL.Conventions;
using Microsoft.Extensions.DependencyInjection.Extensions;

using DocumentExecuter = GraphQL.Conventions.DocumentExecuter;
using ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime;

namespace Examples.Service.Presentation.GraphQL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQLConventions<TQuery, TMutation>(this IServiceCollection services)
        {
            // Graph QL Convention: Engine and Schema
            var engine = new GraphQLEngine()
                .WithFieldResolutionStrategy(FieldResolutionStrategy.Normal)
                .WithQuery<TQuery>()
                .WithMutation<TMutation>()
                .BuildSchema();

            var schema = engine.GetSchema();

            // Add Graph QL Convention Services
            services.AddSingleton(engine);
            services.AddSingleton(schema);
            services.AddTransient<IDependencyInjector, Injector>();

            // Replace GraphQL Server with GraphQL Convention Document Executer
            services.Replace(new ServiceDescriptor(typeof(IDocumentExecuter), typeof(DocumentExecuter), ServiceLifetime.Singleton));

            return services;
        }
    }
}
