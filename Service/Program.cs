using Examples.Service.Endpoints;
using Serilog;
using Serilog.Logfmt;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting();
builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
    {
        p.AllowAnyOrigin();
    });
});
builder.Services.AddHttpContextAccessor();
builder.Host.UseSerilog((context, loggerConfig) => {
    loggerConfig.ReadFrom.Configuration(context.Configuration);
    loggerConfig.WriteTo.Console(
        formatter: new LogfmtFormatter(opt => opt
            .IncludeAllProperties()
            .OnException(e => e
                // Log full stack trace
                .LogStackTrace(LogfmtStackTraceFormat.SingleLine)))
    );
});


var app = builder.Build();
app.UseRouting();
app.UseCors();
app.UseSerilogRequestLogging();
app.UseEndpoints(endpoints =>
{
    endpoints.MapSampleApi();
});
app.Run();
