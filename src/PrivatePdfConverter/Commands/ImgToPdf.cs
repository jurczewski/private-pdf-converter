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
            // todo log supported file types
            return;
        }

        Log.Logger.Information("Read 1 file with name: {FileName}, Full path: '{Path}'", Path.GetFileName(path), path);

        using var images = new MagickImageCollection();
        images.Add(new MagickImage(path));

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

        images.Write($"{Path.GetDirectoryName(path)}/{outputFileName}");

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, $"{path}/{outputFileName}");
    }
}
