var url = Environment.GetEnvironmentVariable("SERVICE_URL");
if (string.IsNullOrEmpty(url))
{
    throw new ArgumentException("SERVICE_URL must be specified");
}

using var httpClient = new HttpClient();
while (true)
{
    try
    {
        var content = await httpClient.GetStringAsync(url);
        Console.WriteLine(content);
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine(ex.Message);
    }

    Thread.Sleep(5000);
}
