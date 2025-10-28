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

        PdfOperationHelper.LogSingleFileRead(path);

        using var images = new MagickImageCollection();
        images.Add(new MagickImage(path));

        PdfOperationHelper.SaveImageCollectionAsPdf(path, images, output, useDirectoryName: true);
    }
}
