using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Examples.Service.Presentation.Endpoints;

public static class SampleEndpoints
{
    public static IEndpointRouteBuilder MapSampleApi(this IEndpointRouteBuilder builder, string prefix = "/")
    {
        builder.MapGet(prefix, async Task<IResult> (ILogger<Program> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) =>
        {
            try
            {
                // .NET Diagnostics: create the span factory
                using var activitySource = new ActivitySource("Examples.Service");

                // .NET Diagnostics: create a metric
                using var meter = new Meter("Examples.Service", "1.0");
                var successCounter = meter.CreateCounter<long>("srv.successes.count", description: "Number of successful responses");

                await ExecuteSql(configuration, "SELECT 1");

                // .NET Diagnostics: create a manual span
                using (var activity = activitySource.StartActivity("SayHello"))
                {
                    activity?.SetTag("foo", 1);
                    activity?.SetTag("bar", "Hello, World!");
                    activity?.SetTag("baz", new int[] { 1, 2, 3 });

                    var waitTime = Random.Shared.NextDouble(); // max 1 seconds
                    await Task.Delay(TimeSpan.FromSeconds(waitTime));

                    activity?.SetStatus(ActivityStatusCode.Ok);

                    logger.LogInformation("Wait time was {waitTime}", waitTime);
                    if (waitTime < 0.5)
                    {
                        throw new Exception($"Error because wait time was {waitTime}");
                    }
                }

                // .NET Diagnostics: update the metric
                successCounter.Add(1);

                logger.LogInformation("Success! Today is: {Date:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Results.StatusCode(500);
            }

            return Results.Ok("Hello there");
        });

        return builder;
    }

    private static async Task ExecuteSql(IConfiguration configuration, string sql)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        using var command = new SqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();
    }
}