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
        if (!FileService.ValidateFileExists(path, "PDF file") || 
            !FileService.ValidateFileExists(logoPath, "Logo image file"))
        {
            return;
        }

        using var pdfDoc = OpenPdfAndPrepareExportFile(path, output, out var exportFullPath);

        var logo = LoadImage(logoPath);
        SetScale(logo, scale);
        SetOpacity(logo, opacity);

        var (x, y) = CalculatePosition(pdfDoc, position, logo);
        AddLogoToPages(pdfDoc, logo, x, y);

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
            Log.Logger.Information("Scale: {Scale}%. Scaled dimensions: {ScaledWidth} x {ScaledHeight}",
                scaleInPercent, logo.GetImageScaledWidth(), logo.GetImageScaledHeight());
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
        exportFullPath = PdfOperationHelper.PrepareOutputPath(path, output, out _);

        var pdf = new PdfDocument(new PdfReader(path), new PdfWriter(exportFullPath));
        Log.Logger.Information("Read a pdf {Path}", path);

        return pdf;
    }

    internal static (float, float) CalculatePosition(PdfDocument pdfDoc, string position, Image logo)
    {
        float x, y; // left, bottom
        var (logoWidth, logoHeight) = (logo.GetImageScaledWidth(), logo.GetImageScaledHeight());

        switch (position.ToLower())
        {
            case "top-left":
                x = 0;
                y = pdfDoc.GetFirstPage().GetPageSize().GetHeight() - logoHeight;
                break;
            case "top-right":
                x = pdfDoc.GetFirstPage().GetPageSize().GetWidth() - logoWidth;
                y = pdfDoc.GetFirstPage().GetPageSize().GetHeight() - logoHeight;
                break;
            case "bottom-left":
                x = 0;
                y = 0;
                break;
            case "bottom-right":
                x = pdfDoc.GetFirstPage().GetPageSize().GetWidth() - logoWidth;
                y = 0;
                break;
            default:
                throw new ArgumentException("Invalid position specified.");
        }

        Log.Logger.Information("For '{Position}' calculated position: {LogoWidth} x {LogoHeight}",
            position, Math.Round(x, 1), Math.Round(y, 1));

        return (x, y);
    }

    private static void AddLogoToPages(PdfDocument pdfDoc, Image logo, float x, float y)
    {
        var numberOfPages = pdfDoc.GetNumberOfPages();
        for (var pageNum = 1; pageNum <= numberOfPages; pageNum++)
        {
            Log.Logger.Information("Adding to page {PageNumber} of {NumberOfPages}", pageNum, numberOfPages);

            logo.SetFixedPosition(pageNum, x, y);

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
