using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class MergePdfIntegrationTests
{
    [Fact]
    public void ShouldCreateMergedPdf_FromPdfsInDirectory()
    {
        var inputDirPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(inputDirPath);
        const string outputName = "merged";
        var outputPdfPath = Path.Combine(inputDirPath, outputName + ".pdf");

        try
        {
            foreach (var pdfName in new[] { "pdf1.pdf", "pdf2.pdf", "pdf3.pdf" })
            {
                using var pdf = new PdfDocument(new PdfWriter(Path.Combine(inputDirPath, pdfName)));
                using var layout = new Document(pdf);
                layout.Add(new Paragraph(pdfName));
            }

            var result = CliTestHelper.Run("merge", "--path", inputDirPath, "--output", outputName);

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(outputPdfPath).Should().BeTrue();

            using var mergedPdf = new PdfDocument(new PdfReader(outputPdfPath));
            mergedPdf.GetNumberOfPages().Should().Be(3);
        }
        finally
        {
            if (Directory.Exists(inputDirPath)) Directory.Delete(inputDirPath, true);
        }
    }
}
