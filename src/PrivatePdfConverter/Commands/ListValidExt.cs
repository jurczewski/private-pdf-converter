using PrivatePdfConverter.Services;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class ListValidExt
{
    /// <summary>
    /// Lists valid image extensions
    /// </summary>
    public static void ListValidExtensions()
        => Log.Logger.Information("Valid image extension: {Extension}", string.Join(", .", FileService.ValidExtensions));
}
