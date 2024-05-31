using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using PrivatePdfConverter.Services;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class AddLogoToPdf
{
    public static void Run(string path, string logoPath, string position, int? scale, int? opacity, string? output)
    {
        if (!File.Exists(path))
        {
            Log.Logger.Error("PDF file '{Path}' does not exist", path);
            return;
        }

        if (!File.Exists(logoPath))
        {
            Log.Logger.Error("Logo image file '{LogoPath}' does not exist", logoPath);
            return;
        }

        using var pdfDoc = OpenPdfAndPrepareExportFile(path, output, out var exportFullPath);

        var logo = LoadImage(logoPath);
        SetScale(logo, scale);
        SetOpacity(logo, opacity);
        AddLogoToPages(pdfDoc, logo);

        Log.Logger.Information("Created a new pdf at {ExportFullPath}", exportFullPath);
    }

    private static void SetOpacity(Image logo, int? opacity)
    {
        if (opacity.HasValue)
        {
            logo.SetOpacity(opacity.Value / 100f);
            Log.Logger.Information("Opacity: {Opacity}%", opacity);
        }
    }

    private static void SetScale(Image logo, int? scaleInPercent)
    {
        if (scaleInPercent.HasValue)
        {
            var scaleFloat = scaleInPercent.Value / 100f;
            logo.Scale(scaleFloat, scaleFloat);
            Log.Logger.Information("Scale: {Scale}%", scaleInPercent);
        }
        else
        {
            logo.SetAutoScaleWidth(true);
            logo.SetAutoScaleHeight(true);
        }
    }

    private static Image LoadImage(string logoPath)
    {
        var logo = new Image(ImageDataFactory.Create(logoPath));
        Log.Logger.Information("Read logo {Path}, image dimensions: {Width} x {Height}", logoPath, logo.GetImageWidth(), logo.GetImageHeight());
        return logo;
    }

    private static PdfDocument OpenPdfAndPrepareExportFile(string path, string? output, out string exportFullPath)
    {
        var outputFileName = output.PrepareOutputFileName();
        exportFullPath = Path.GetDirectoryName(path).AddFileToPath(outputFileName);

        var pdf = new PdfDocument(new PdfReader(path), new PdfWriter(exportFullPath));
        Log.Logger.Information("Read a pdf from {Path}", path);

        return pdf;
    }

    private static void AddLogoToPages(PdfDocument pdfDoc, Image logo)
    {
        var numberOfPages = pdfDoc.GetNumberOfPages();
        for (var pageNum = 1; pageNum <= numberOfPages; pageNum++)
        {
            Log.Logger.Information("Adding to page {PageNumber} of {NumberOfPages}", pageNum, numberOfPages);

            logo.SetFixedPosition(pageNum, 0, 0);

            var pdfPage = pdfDoc.GetPage(pageNum);
            var pdfCanvas = new PdfCanvas(
                pdfPage.NewContentStreamBefore(),
                pdfPage.GetResources(),
                pdfDoc);

            using var canvas = new Canvas(pdfCanvas, pdfPage.GetPageSize());
            canvas.Add(logo);
        }
    }
}
