using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace PrivatePdfConverter;

public static class Logger
{
    public static void Initialize()
        => Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();
}