using ImageMagick;
using Serilog;

namespace PrivatePdfConverter.Services;

public static class FileService
{
    // Conservative defaults chosen to allow typical high-resolution images
    // while rejecting obviously pathological dimensions before full decode.
    private const ulong MaxWidth = 10_000;
    private const ulong MaxHeight = 10_000;
    private const ulong MaxArea = 100_000_000;

    public static IEnumerable<string> ValidExtensions { get; } =
    [
        "jpg", "jpeg", "bmp", "gif", "png", "tif", "tiff", "webp"
    ];

    public static bool IsImage(this string? extension)
        => !string.IsNullOrEmpty(extension) && ValidExtensions.Contains(extension.ToLower()[1..]);

    public static MagickImage? LoadValidatedImage(string path)
    {
        using var headerImage = new MagickImage();
        headerImage.Ping(path);

        var width = headerImage.Width;
        var height = headerImage.Height;
        var area = (ulong)width * height;

        if (width == 0 || height == 0)
        {
            Log.Logger.Error("Image '{Path}' has invalid dimensions {Width}x{Height}", path, width, height);
            return null;
        }

        if (width > MaxWidth || height > MaxHeight || area > MaxArea)
        {
            Log.Logger.Error(
                "Image '{Path}' exceeds the supported limits ({Width}x{Height}, area {Area})",
                path,
                width,
                height,
                area);
            return null;
        }

        return new MagickImage(path);
    }

    public static IEnumerable<string> LoadFilePathsFromDirectory(this string path, string searchPattern = "*")
    {
        var files = Directory.GetFiles(path, searchPattern).ToList();

        Log.Logger.Information("Read {Count} files from '{Path}'", files.Count, path);
        files.ForEach(x => Log.Logger.Information("File name: {FileName}", Path.GetFileName(x)));

        return files;
    }

    /// <summary>
    /// Prepares the output file name (with the .pdf extension) based on the provided output
    /// or source path. See <see cref="PrepareOutputBaseName"/> for the naming rules.
    /// Examples: "C:\\images\\photo.jpg" -> "photo.pdf", "report.pdf" -> "report_export.pdf"
    /// </summary>
    public static string PrepareOutputFileName(this string? output, string sourcePath)
        => output.PrepareOutputBaseName(sourcePath) + ".pdf";

    /// <summary>
    /// Prepares the output base name (without the .pdf extension) based on the provided
    /// output or source path. This is the shared building block behind
    /// <see cref="PrepareOutputFileName"/>; use it when the caller needs to append its own
    /// suffix (e.g. a page range) before adding the extension.
    /// Examples: "report.pdf" -> "report", "C:\\images\\photo.jpg" -> "photo",
    /// null with source "report.pdf" -> "report_export".
    /// </summary>
    public static string PrepareOutputBaseName(this string? output, string sourcePath)
    {
        if (!string.IsNullOrEmpty(output))
        {
            return output.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)
                ? Path.GetFileNameWithoutExtension(output)
                : output;
        }

        var normalizedSource = sourcePath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var sourceFileName = Path.GetFileNameWithoutExtension(normalizedSource);
        var baseName = string.IsNullOrEmpty(sourceFileName) ? "output" : sourceFileName;

        // prevent read+write on the same file when source is already a PDF
        return string.Equals(baseName + ".pdf", Path.GetFileName(sourcePath), StringComparison.OrdinalIgnoreCase)
            ? baseName + "_export"
            : baseName;
    }
}
