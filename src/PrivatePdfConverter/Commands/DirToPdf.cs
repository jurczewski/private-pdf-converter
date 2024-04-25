using ImageMagick;
using PrivatePdfConverter.Services;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class DirToPdf
{
    public static void ConvertDirectoryToOnePdf(string path, string? output)
    {
        var filesPaths = path.LoadFilePathsFromDirectory();
        var supportedFiles = filesPaths.Where(x => Path.GetExtension(x).IsImage()).ToList();

        using var images = new MagickImageCollection();
        supportedFiles.ForEach(x => images.Add(new MagickImage(x)));

        SaveAsPdf(path, images, output);
    }

    private static void SaveAsPdf(string path, MagickImageCollection images, string? output)
    {
        var outputFileName = output.PrepareOutputFileName();
        var fileWithPath = path.AddFileToPath(outputFileName);

        images.Write(fileWithPath);

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, fileWithPath);
    }
}
