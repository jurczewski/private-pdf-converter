using System.Diagnostics;

namespace PrivatePdfConverter.Tests.IntegrationTests;

internal static class CliTestHelper
{
    private static readonly string appAssemblyPath =
        Path.Combine(AppContext.BaseDirectory, "PrivatePdfConverter.dll");

    internal static CliResult Run(params string[] args)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        startInfo.ArgumentList.Add(appAssemblyPath);
        foreach (var arg in args)
        {
            startInfo.ArgumentList.Add(arg);
        }

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start the dotnet process.");

        var stdOut = process.StandardOutput.ReadToEndAsync();
        var stdErr = process.StandardError.ReadToEndAsync();

        if (!process.WaitForExit(60_000))
        {
            process.Kill(entireProcessTree: true);
            throw new TimeoutException($"CLI test timed out after 60s. Args: {string.Join(" ", args)}");
        }

        Task.WhenAll(stdOut, stdErr).GetAwaiter().GetResult();

        return new CliResult(process.ExitCode, stdOut.Result, stdErr.Result);
    }
}

internal sealed record CliResult(int ExitCode, string StandardOutput, string StandardError);
