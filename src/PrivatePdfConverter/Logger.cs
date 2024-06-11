using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace PrivatePdfConverter;

public static class Logger
{
    public static void Initialize()
        => Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();

    public static void LogStart(string versionString)
        => Log.Logger.Information("--- PrivatePdfConverter v{VersionString} ---", versionString);
}
