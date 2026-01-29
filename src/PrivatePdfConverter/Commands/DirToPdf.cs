using ImageMagick;
using PrivatePdfConverter.Services;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class DirToPdf
{
    /// <summary>
    /// Convert all images in the specified path into a single PDF file.
    /// </summary>
    /// <param name="path">Path to the directory containing images.</param>
    /// <param name="output">Optional output file name (without extension).</param>
    public static void ConvertDirectoryToOnePdf(string path, string? output)
    {
        var filesPaths = path.LoadFilePathsFromDirectory();
        var supportedFiles = filesPaths.Where(x => Path.GetExtension(x).IsImage()).ToList();

        if (supportedFiles.Count == 0)
        {
            Log.Logger.Warning("No supported image files found in directory '{Path}'. To merge PDF files, use the 'merge' command instead", path);
            return;
        }

        using var images = new MagickImageCollection();
        supportedFiles.ForEach(x => images.Add(new MagickImage(x)));

        SaveAsPdf(path, images, output);
    }

    private static void SaveAsPdf(string path, MagickImageCollection images, string? output)
    {
        var outputFileName = output.PrepareOutputFileName(path);
        var fileWithPath = Path.Combine(path, outputFileName);

        images.Write(fileWithPath);

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, fileWithPath);
    }
}
