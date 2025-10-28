using ImageMagick;
using PrivatePdfConverter.Services;

namespace PrivatePdfConverter.Commands;

public static class DirToPdf
{
    public static void ConvertDirectoryToOnePdf(string path, string? output)
    {
        var filesPaths = path.LoadFilePathsFromDirectory();
        var supportedFiles = filesPaths.Where(x => Path.GetExtension(x).IsImage()).ToList();

        using var images = new MagickImageCollection();
        supportedFiles.ForEach(x => images.Add(new MagickImage(x)));

        PdfOperationHelper.SaveImageCollectionAsPdf(path, images, output);
    }
}
