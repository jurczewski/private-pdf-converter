using Serilog;

namespace PrivatePdfConverter.Services;

public static class FileService
{
    public static IEnumerable<string> ValidExtensions { get; } =
    [
        "jpg", "jpeg", "bmp", "gif", "png", "tif", "tiff", "webp"
    ];

    public static bool IsImage(this string? extension)
        => !string.IsNullOrEmpty(extension) && ValidExtensions.Contains(extension.ToLower()[1..]);

    public static IEnumerable<string> LoadFilePathsFromDirectory(this string path, string searchPattern = "*")
    {
        var files = Directory.GetFiles(path, searchPattern).ToList();

        Log.Logger.Information("Read {Count} files from '{Path}'", files.Count, path);
        files.ForEach(x => Log.Logger.Information("File name: {FileName}", Path.GetFileName(x)));

        return files;
    }

    /// <summary>
    /// Prepares the output file name based on the provided output or source path.
    /// If `output` is provided, use it (and ensure .pdf). If not provided, derive the name from the `sourcePath` filename.
    /// Example: sourcePath = "C:\\images\\photo.jpg" -> "photo.pdf"
    /// </summary>
    /// <param name="output"></param>
    /// <param name="sourcePath"></param>
    /// <returns></returns>
    public static string PrepareOutputFileName(this string? output, string sourcePath)
    {
        if (!string.IsNullOrEmpty(output))
        {
            return output.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)
                ? output
                : output + ".pdf";
        }

        // derive from the source path filename
        var sourceFileName = Path.GetFileNameWithoutExtension(sourcePath);
        var outputFileName = string.IsNullOrEmpty(sourceFileName) ? "output" : sourceFileName;
        return outputFileName + ".pdf";
    }

    public static string AddFileToPath(this string? path, string fileName)
        => !string.IsNullOrEmpty(path) ? Path.Combine(path, fileName) : fileName;
}
