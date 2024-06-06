using AutoFixture;
using FluentAssertions;
using ImageMagick;
using iText.Kernel.Pdf;
using PrivatePdfConverter.Commands;
using Xunit;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class AddLogoToPdfIntegrationTests : IDisposable
{
    private const string OutputPdfFileName = "output.pdf";
    private readonly string _samplePdfPath;
    private readonly string _logoPath;
    private readonly string _outputPdfPath;

    public AddLogoToPdfIntegrationTests()
    {
        var fixture = new Fixture();
        var samplePdfName = fixture.Create<string>();
        _samplePdfPath = Path.Combine(Path.GetTempPath(), samplePdfName + ".pdf");
        _outputPdfPath = Path.Combine(Path.GetTempPath(), OutputPdfFileName);
        _logoPath = Path.Combine(Path.GetTempPath(), "logo.png");
    }


    [Fact]
    public void Run_AddsLogoToPdf_ChecksIfLogoPresent()
    {
        // Arrange
        CreateSamplePdf(_samplePdfPath);
        CreateSampleLogo(_logoPath);

        // Act
        AddLogoToPdf.Run(_samplePdfPath, _logoPath, "top-left", 25, 100, OutputPdfFileName);

        // Assert
        using var pdfReader = new PdfReader(_outputPdfPath);
        using var pdfDocument = new PdfDocument(pdfReader);
        const int pageNumber = 1; // Assuming the logo is added to the first page
        var page = pdfDocument.GetPage(pageNumber);
        var imageXObject = page.GetPdfObject().GetAsDictionary(PdfName.Resources)?.GetAsDictionary(PdfName.XObject)?.GetAsStream(new PdfName("Im1"));

        imageXObject.Should().NotBeNull("because the logo should be present in the PDF");
    }

    public void Dispose()
    {
        if (File.Exists(_samplePdfPath))
            File.Delete(_samplePdfPath);

        if (File.Exists(_outputPdfPath))
            File.Delete(_outputPdfPath);

        if (File.Exists(_logoPath))
            File.Delete(_logoPath);
    }

    private static void CreateSamplePdf(string filePath)
    {
        using var pdfWriter = new PdfWriter(filePath);
        using var pdfDocument = new PdfDocument(pdfWriter);
        var document = new iText.Layout.Document(pdfDocument);
        document.Add(new iText.Layout.Element.Paragraph("This is a sample PDF."));
    }

    private static void CreateSampleLogo(string filePath)
    {
        using var image = new MagickImage(MagickColors.Red, 100, 100);
        image.Write(filePath);
    }
}
