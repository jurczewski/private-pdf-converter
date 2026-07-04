using System.Text;
using iText.Kernel.Pdf;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class EncryptPdfIntegrationTests
{
    [Fact]
    public void ShouldCreatePasswordProtectedPdf_WhenOutputIsOmitted()
    {
        var inputPdfPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.pdf");
        const string password = "test-password-123";
        var outputPdfPath = Path.Combine(
            Path.GetDirectoryName(inputPdfPath)!,
            Path.GetFileNameWithoutExtension(inputPdfPath) + "_export.pdf");

        try
        {
            CreateSamplePdf(inputPdfPath);

            var result = CliTestHelper.Run(
                "encrypt",
                "--path", inputPdfPath,
                "--password", password);

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(outputPdfPath).Should().BeTrue();

            var openWithCorrectPassword = () =>
            {
                using var reader = new PdfReader(outputPdfPath, new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(password)));
                using var pdf = new PdfDocument(reader);
            };
            openWithCorrectPassword.Should().NotThrow();
        }
        finally
        {
            if (File.Exists(inputPdfPath)) File.Delete(inputPdfPath);
            if (File.Exists(outputPdfPath)) File.Delete(outputPdfPath);
        }
    }

    private static void CreateSamplePdf(string filePath)
    {
        using var writer = new PdfWriter(filePath);
        using var pdf = new PdfDocument(writer);
        var doc = new iText.Layout.Document(pdf);
        doc.Add(new iText.Layout.Element.Paragraph("Sample PDF for encryption test."));
    }
}
