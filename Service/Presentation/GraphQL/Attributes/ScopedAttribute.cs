using GraphQL;
using GraphQL.Conventions;
using GraphQL.Conventions.Attributes;
using GraphQL.Conventions.Execution;

namespace Examples.Service.Presentation.GraphQL.Attributes
{
    /// <summary>
    /// This attribute is used to create a new scope for each field execution. It is meant to be used to decorate methods in the query and mutation classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ScopedAttribute : ExecutionFilterAttributeBase
    {
        public override async Task<object> Execute(IResolutionContext context, FieldResolutionDelegate next)
        {
            var scopeFactory = context.DependencyInjector.Resolve<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            var scopedDependecyInjector = new DependencyInjector(scope.ServiceProvider);
            context.FieldContext.SetDependencyInjector(scopedDependecyInjector);
            return await next(context);
        }
    }
}
