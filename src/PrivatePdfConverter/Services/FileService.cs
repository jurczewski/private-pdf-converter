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

    public static string AddFileToPath(this string? path, string fileName) => $"{path}{Path.DirectorySeparatorChar}{fileName}";

    /// <summary>
    /// Validates if a file exists and logs an error if it doesn't
    /// </summary>
    /// <param name="filePath">Path to the file to validate</param>
    /// <param name="fileDescription">Description of the file for error logging</param>
    /// <returns>True if file exists, false otherwise</returns>
    public static bool ValidateFileExists(string filePath, string fileDescription)
    {
        if (!File.Exists(filePath))
        {
            Log.Logger.Error("{FileDescription} '{FilePath}' does not exist", fileDescription, filePath);
            return false;
        }
        return true;
    }
}
