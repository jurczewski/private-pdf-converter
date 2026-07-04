using ImageMagick;
using iText.Kernel.Pdf;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class AddLogoToPdfIntegrationTests
{
    [Fact]
    public void ShouldAddLogoToPdf_WithExplicitScaleAndOpacity()
    {
        var inputPdfPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.pdf");
        var logoPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
        var outputName = Guid.NewGuid().ToString("N");
        var outputPdfPath = Path.Combine(Path.GetDirectoryName(inputPdfPath)!, outputName + ".pdf");

        try
        {
            CreateSamplePdf(inputPdfPath);
            CreateLogoImage(logoPath);

            var result = CliTestHelper.Run(
                "logo",
                "--path", inputPdfPath,
                "--logo-path", logoPath,
                "--position", "top-left",
                "--scale", "25",
                "--opacity", "50",
                "--output", outputName);

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(outputPdfPath).Should().BeTrue();

            using var reader = new PdfReader(outputPdfPath);
            using var pdf = new PdfDocument(reader);
            var xObject = pdf.GetPage(1)
                .GetPdfObject()
                .GetAsDictionary(PdfName.Resources)
                ?.GetAsDictionary(PdfName.XObject)
                ?.GetAsStream(new PdfName("Im1"));
            xObject.Should().NotBeNull("because the logo should be embedded even when scale and opacity are specified");
        }
        finally
        {
            if (File.Exists(inputPdfPath)) File.Delete(inputPdfPath);
            if (File.Exists(logoPath)) File.Delete(logoPath);
            if (File.Exists(outputPdfPath)) File.Delete(outputPdfPath);
        }
    }

    [Fact]
    public void ShouldAddLogoToPdf_WhenAllOptionalsAreOmitted()
    {
        var inputPdfPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.pdf");
        var logoPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
        var outputPdfPath = Path.Combine(
            Path.GetDirectoryName(inputPdfPath)!,
            Path.GetFileNameWithoutExtension(inputPdfPath) + "_export.pdf");

        try
        {
            CreateSamplePdf(inputPdfPath);
            CreateLogoImage(logoPath);

            var result = CliTestHelper.Run(
                "logo",
                "--path", inputPdfPath,
                "--logo-path", logoPath,
                "--position", "top-left");

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(outputPdfPath).Should().BeTrue();

            using var reader = new PdfReader(outputPdfPath);
            using var pdf = new PdfDocument(reader);
            var xObject = pdf.GetPage(1)
                .GetPdfObject()
                .GetAsDictionary(PdfName.Resources)
                ?.GetAsDictionary(PdfName.XObject)
                ?.GetAsStream(new PdfName("Im1"));
            xObject.Should().NotBeNull("because the logo should be embedded as an XObject in the PDF");
        }
        finally
        {
            if (File.Exists(inputPdfPath)) File.Delete(inputPdfPath);
            if (File.Exists(logoPath)) File.Delete(logoPath);
            if (File.Exists(outputPdfPath)) File.Delete(outputPdfPath);
        }
    }

    private static void CreateSamplePdf(string filePath)
    {
        using var writer = new PdfWriter(filePath);
        using var pdf = new PdfDocument(writer);
        var doc = new iText.Layout.Document(pdf);
        doc.Add(new iText.Layout.Element.Paragraph("Sample PDF for logo test."));
    }

    private static void CreateLogoImage(string filePath)
    {
        using var image = new MagickImage(MagickColors.Red, 100, 100);
        image.Write(filePath);
    }
}
