using iText.Kernel.Pdf;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class MergePdf
{
    public static void ConvertDirectoryToOnePdf(string path, string? output)
    {
        var filesPaths = Directory.GetFiles(path, "?.pdf").ToList();

        Log.Logger.Information("Read {Count} files from '{Path}'", filesPaths.Count, path);
        filesPaths.ForEach(x => Log.Logger.Information("File name: {FileName}", Path.GetFileName(x)));
        var pdfs = filesPaths.Select(x => new PdfDocument(new PdfReader(x))).ToList();
        var outputFileName = string.IsNullOrEmpty(output) ? "output.pdf" : output;

        // check if filename already has .pdf extension and add .pdf at the end if
        if (!outputFileName.EndsWith(".pdf"))
        {
            outputFileName += ".pdf";
        }

        using var mergedDocument = new PdfDocument(new PdfWriter($"{path}/{outputFileName}"));
        foreach (var pdf in pdfs)
        {
            pdf.CopyPagesTo(1, pdf.GetNumberOfPages(), mergedDocument);
        }

        pdfs.ForEach(x => x.Close());

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, $"{path}/{outputFileName}");
    }
}
