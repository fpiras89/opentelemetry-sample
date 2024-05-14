using Examples.Service.Application.Interfaces;
using Examples.Service.Application.Metrics;
using Examples.Service.Application.Services;
using Examples.Service.Infrastructure;
using Examples.Service.Persistence;
using Examples.Service.Presentation.Endpoints;
using Examples.Service.Presentation.GraphQL;
using GraphQL;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(b => b
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), options => 
    {
        options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
    }));
builder.Services.AddScoped<IDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

builder.Services.AddRouting();

builder.Services.AddGraphQL(b => b
    .AddAutoSchema<Query>(configure => configure.WithMutation<Mutation>())
    .AddNewtonsoftJson()
    .AddErrorInfoProvider(options => options.ExposeExceptionDetails = true)
    .UseApolloTracing()
    .UseTelemetry()
    .ConfigureExecutionOptions(options =>
    {
        var logger = options.RequestServices.GetRequiredService<ILogger<Program>>();
        options.UnhandledExceptionDelegate = ctx =>
        {
            logger.LogError("GraphQL Unhandled Exception: {ErrorMessage} | {OriginalExceptionMessage}", ctx.ErrorMessage, ctx.OriginalException.Message);
            return Task.CompletedTask;
        };
    }));

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
    {
        p.AllowAnyOrigin();
        p.AllowAnyHeader();
        p.AllowAnyMethod();
    });
});

builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog((context, loggerConfig) => 
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
    /*loggerConfig.WriteTo.Console(
        formatter: new LogfmtFormatter(opt => opt
            //.IncludeAllProperties()
            .OnException(e => e
                // Log full stack trace
                .LogStackTrace(LogfmtStackTraceFormat.SingleLine)))
    );*/
});

builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddHostedService<MigrationsHostedService>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddMetrics();
builder.Services.AddSingleton<TodoMetrics>();

var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL("graphql");
    endpoints.MapGraphQLPlayground("graphql/playground");
    endpoints.MapSampleApi();
    endpoints.MapToDoApi();
    endpoints.MapHealthChecks("/health");
});
app.Run();
