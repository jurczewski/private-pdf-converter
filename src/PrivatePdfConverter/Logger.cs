using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace PrivatePdfConverter;

public static class Logger
{
    public static void Initialize()
        => Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();

    public static void LogStart(string fullVersionString)
    {
        // Split the version string by '+' to get the version and the commit ID
        var versionParts = fullVersionString.Split('+');
        var shortVersion = versionParts[0];
        var commitId = versionParts.Length > 1 ? versionParts[1] : "0.0.0";

        // Log the message
        Log.Logger.Information("--- PrivatePdfConverter v{VersionString} (commit: {CommitId}) ---", shortVersion, commitId);
    }
}
