using ImageMagick;
using PrivatePdfConverter.Services;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class ImgToPdf
{
    /// <summary>
    /// Convert a single image file to a PDF.
    /// </summary>
    /// <param name="path">Path to the image file.</param>
    /// <param name="output">Optional output file name (without extension).</param>
    public static void ConvertImageToOnePdf(string path, string? output)
    {
        if (!Path.GetExtension(path).IsImage())
        {
            Log.Logger.Fatal("File type not supported. Please provide a valid image file");
            ListValidExt.ListValidExtensions();
            return;
        }

        Log.Logger.Information("Read 1 file with name: {FileName}, Full path: '{Path}'", Path.GetFileName(path), path);

        using var images = new MagickImageCollection();
        images.Add(new MagickImage(path));

        SaveAsPdf(path, images, output);
    }

    private static void SaveAsPdf(string path, MagickImageCollection images, string? output)
    {
        var outputFileName = output.PrepareOutputFileName(path);

        var fileWithPath = Path.Combine(Path.GetDirectoryName(path) ?? string.Empty, outputFileName);
        images.Write(fileWithPath);

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, fileWithPath);
    }
}
