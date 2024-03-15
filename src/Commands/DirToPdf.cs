using ImageMagick;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class DirToPdf
{
    public static void ConvertDirectoryToOnePdf(string path, string? output)
    {
        var files = LoadFilesFromPath(path);

        using var images = new MagickImageCollection();
        files.ForEach(x => images.Add(new MagickImage(x)));

        SaveAsPdf(path, output, images);
    }

    private static void SaveAsPdf(string path, string? output, MagickImageCollection images)
    {
        var outputFileName = output ?? "output"; // make default output file name

        // check if filename already has .pdf extension and add .pdf at the end if
        if (!outputFileName.EndsWith(".pdf"))
        {
            outputFileName = $"{outputFileName}.pdf"; // use += to append .pdf
        }

        images.Write($"{path}/{outputFileName}");

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, $"{path}/{outputFileName}");
    }

    private static List<string> LoadFilesFromPath(string path)
    {
        var files = Directory.GetFiles(path, "?.png").ToList();

        Log.Logger.Information("Read {Count} files from '{Path}'", files.Count, path);
        files.ForEach(x => Log.Logger.Information("File name: {FileName}", Path.GetFileName(x)));

        return files;
    }
}
