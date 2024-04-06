using Serilog;

namespace PrivatePdfConverter.Services;

public static class FileService
{
    private static readonly string[] validExtensions =
    [
        "jpg", "jpeg", "bmp", "gif", "png", "tif", "tiff", "webp"
    ];

    public static IEnumerable<string> LoadFilePathsFromDirectory(this string path, string searchPattern = "*")
    {
        var files = Directory.GetFiles(path, searchPattern).ToList();

        Log.Logger.Information("Read {Count} files from '{Path}'", files.Count, path);
        files.ForEach(x => Log.Logger.Information("File name: {FileName}", Path.GetFileName(x)));

        return files;
    }

    public static bool IsImage(this string extension) => validExtensions.Contains(extension.ToLower()[1..]);
}
