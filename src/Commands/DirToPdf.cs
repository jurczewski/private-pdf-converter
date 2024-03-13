using ImageMagick;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class DirToPdf
{
    public static void ConvertDirectoryToOnePdf(string path, string? output)
    {
        var files = Directory.GetFiles(path, "?.png").ToList();

        Log.Logger.Information("Read {Count} files from '{Path}'", files.Count, path);
        files.ForEach(x => Log.Logger.Information("File name: {FileName}", Path.GetFileName(x)));

        using var images = new MagickImageCollection();
        files.ForEach(x => images.Add(new MagickImage(x)));

        var outputFileName = output ?? "output";

        // check if filename already has .pdf extension and add .pdf at the end if
        if (!outputFileName.EndsWith(".pdf"))
        {
            outputFileName = $"{outputFileName}.pdf";
        }

        images.Write($"{path}/{outputFileName}");

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, $"{path}/{outputFileName}");
    }
}
