using ImageMagick;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class DirToPdf
{
    public static void ConvertDirectoryToOnePdf(string path, string? output)
    {
        var filesPaths = LoadFilePathsFromDirectory(path);

        using var images = new MagickImageCollection();
        filesPaths.ForEach(x => images.Add(new MagickImage(x)));

        SaveAsPdf(path, images, output);
    }

    private static void SaveAsPdf(string path, MagickImageCollection images, string? output)
    {
        var outputFileName = string.IsNullOrEmpty(output) ? "output.pdf" : output;

        // check if filename already has .pdf extension and add .pdf at the end if
        if (!outputFileName.EndsWith(".pdf"))
        {
            outputFileName += ".pdf";
        }

        images.Write($"{path}/{outputFileName}");

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, $"{path}/{outputFileName}");
    }

    private static List<string> LoadFilePathsFromDirectory(string path)
    {
        var files = Directory.GetFiles(path);
        var supportedFiles = files.Where(x => IsImage(Path.GetExtension(x))).ToList();

        Log.Logger.Information("Read {Count} files from '{Path}'", supportedFiles.Count, path);
        supportedFiles.ForEach(x => Log.Logger.Information("File name: {FileName}", Path.GetFileName(x)));

        return supportedFiles;
    }

    private static bool IsImage(string extension)
    {
        var validExtensions = new[]
        {
            "jpg", "jpeg", "bmp", "gif", "png", "tif", "tiff", "webp"
        };
        return validExtensions.Contains(extension.ToLower()[1..]);
    }
}
