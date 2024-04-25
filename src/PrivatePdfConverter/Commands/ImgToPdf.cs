using ImageMagick;
using PrivatePdfConverter.Services;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class ImgToPdf
{
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
        var outputFileName = output.PrepareOutputFileName();

        images.Write($"{Path.GetDirectoryName(path).AddFileToPath(outputFileName)}");

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, path.AddFileToPath(outputFileName));
    }
}
