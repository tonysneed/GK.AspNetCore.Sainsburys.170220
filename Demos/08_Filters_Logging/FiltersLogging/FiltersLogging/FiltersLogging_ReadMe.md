# FiltersLogging ReadMe

1. Comment out the line in Program.Main that adds hosting.json

    ```csharp
    //.AddJsonFile("hosting.json", optional:true)
    ```

    - Add a profile to launchSettings.json in the Properties folder

    ```json
    "FiltersLogging (Production)": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Production"
      }
    }
    ```

    - Select the Production profile and press Ctrl+F5
      + You should see the app launch in Production environment

2. Add a ThrowError action to the Home controller

    ```csharp
    [Route("/throw")]
    public IActionResult ThrowError()
    {
        throw new Exception("Doh!");
    }
    ```

    - With the app running in Production, 
      navigate to http://localhost:5000/throw
      + You'll just get a 500 response in the browser
      + The console will show the error stack trace

    - Relaunch the app in Development and refresh the web page.
      + A detailed error page will show in the browser

3. Add Error.cshtml to Views/Shared folder

    ```html
    <!DOCTYPE html>
    <html>
    <head>
        <title>Error</title>
    </head>
    <body>
        <h1>Error</h1>
        <h2>An error occurred while processing your request.</h2>
        <p>Swapping to <strong>Development</strong> environment will display more detailed information about the error that occurred.</p>
    </body>
    </html>
    ```

    - Add an Error action to the Home controller

    ```csharp
    public IActionResult Error()
    {
        return View();
    }
    ```

    - Add an else block in Configure to use the error page

    ```csharp
    else
    {
        app.UseExceptionHandler("/Home/Error");
    }
    ```

    - Try running the app again in dev and prod with /throw to see errors

4. To add error logging we'll use an exception filter

    - Add ExceptionLoggingFilter.cs to a Filters folder
    - Extend ExceptionFilter
    - Inject ILogger<ExceptionLoggingFilter> into a ctor
    - Override OnException

    ```csharp
    public class ExceptionLoggingFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionLoggingFilter> _logger;

        public ExceptionLoggingFilter(ILogger<ExceptionLoggingFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError($"Exception: {context.Exception.Message}", context.Exception);
        }
    }
    ```

5. Register the exception filter in Add.Mvc method in Startup.ConfigureServices

    ```csharp
    services.AddMvc(options =>
    {
        options.Filters.Add(new TypeFilterAttribute(typeof(ExceptionLoggingFilter)));
    });
    ```

    - If you run the app in Production, you'll see a lot of output, 
      making it difficult to see our custom error logging.

    - Refactor the call to loggerFactory.AddConsole in Startup.Configure 
      to pass a new ConsoleLoggerSettings with a Switches property that 
      sets the log level of the Microsoft logger to None.
        + Set our own logger level to Debug.

    ```csharp
    loggerFactory.AddConsole(new ConsoleLoggerSettings
    {
        Switches = new Dictionary<string, LogLevel>
        {
            { "Microsoft", LogLevel.None },
            { "FiltersLogging", LogLevel.Debug },
        }
    });
    ```

*Optional: Use NLog for logging exceptions:* <http://dotnetthoughts.net/using-nlog-in-aspnet-core/>