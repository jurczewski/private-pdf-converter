using Serilog;

namespace PrivatePdfConverter.Services;

public static class FileService
{
    public static IEnumerable<string> ValidExtensions { get; } =
    [
        "jpg", "jpeg", "bmp", "gif", "png", "tif", "tiff", "webp"
    ];

    public static bool IsImage(this string extension) => ValidExtensions.Contains(extension.ToLower()[1..]);

    public static IEnumerable<string> LoadFilePathsFromDirectory(this string path, string searchPattern = "*")
    {
        var files = Directory.GetFiles(path, searchPattern).ToList();

        Log.Logger.Information("Read {Count} files from '{Path}'", files.Count, path);
        files.ForEach(x => Log.Logger.Information("File name: {FileName}", Path.GetFileName(x)));

        return files;
    }

    public static string PrepareOutputFileName(this string? output)
    {
        var outputFileName = string.IsNullOrEmpty(output) ? "output.pdf" : output;

        // check if filename already has .pdf extension, if not add .pdf at the end
        if (!outputFileName.EndsWith(".pdf"))
        {
            outputFileName += ".pdf";
        }

        return outputFileName;
    }

    public static string AddFileToPath(this string? path, string fileName) => $"{path}\\{fileName}";
}
