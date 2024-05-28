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
    public static void Run(string path, string logoPath, string position, string? output)
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

        var outputFileName = output.PrepareOutputFileName();
        var exportFullPath = Path.GetDirectoryName(path).AddFileToPath(outputFileName);
        Log.Logger.Information("Read a pdf from {Path}", path);
        using var pdfDoc = new PdfDocument(new PdfReader(path), new PdfWriter(exportFullPath));

        var logo = new Image(ImageDataFactory.Create(logoPath));
        Log.Logger.Information("Read logo {Path}, image dimensions: {Width} x {Height}", logoPath, logo.GetImageWidth(), logo.GetImageHeight());

        AddLogoToPages(pdfDoc, logo);

        pdfDoc.Close();
        Log.Logger.Information("Created a new pdf at {ExportFullPath}", exportFullPath);
    }

    private static void AddLogoToPages(PdfDocument pdfDoc, Image logo)
    {
        var numberOfPages = pdfDoc.GetNumberOfPages();
        for (var pageNum = 1; pageNum <= numberOfPages; pageNum++)
        {
            Log.Logger.Information("Adding to page {PageNumber} of {NumberOfPages}", pageNum, numberOfPages);
            logo.SetFixedPosition(pageNum, 0, 0);
            logo.SetAutoScaleWidth(true);
            logo.SetAutoScaleHeight(true);

            var pdfPage = pdfDoc.GetPage(pageNum);
            var pdfCanvas = new PdfCanvas(
                pdfPage.NewContentStreamBefore(),
                pdfPage.GetResources(),
                pdfDoc);

            using var canvas = new Canvas(pdfCanvas, pdfPage.GetPageSize());
            canvas.Add(logo)
                .Close();
        }
    }
}
