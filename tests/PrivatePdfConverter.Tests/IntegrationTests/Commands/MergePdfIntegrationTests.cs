using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using PrivatePdfConverter.Commands;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class MergePdfIntegrationTests
{
    [Fact]
    public void ConvertDirectoryToOnePdf_ShouldMergePdfsCorrectly()
    {
        // Arrange
        var inputDirPath = Path.Combine(Path.GetTempPath(), "MergePdfTest");
        Directory.CreateDirectory(inputDirPath);

        const string outputFileName = "merged.pdf";

        // Create dummy PDF files
        var pdfPaths = new[]
        {
            Path.Combine(inputDirPath, "pdf1.pdf"),
            Path.Combine(inputDirPath, "pdf2.pdf"),
            Path.Combine(inputDirPath, "pdf3.pdf")
        };

        foreach (var pdfPath in pdfPaths)
        {
            using var pdf = new PdfDocument(new PdfWriter(pdfPath));
            var document = new Document(pdf);
            document.Add(new Paragraph($"Source: {pdfPath}"));
        }

        // Act
        MergePdf.ConvertDirectoryToOnePdf(inputDirPath, outputFileName);

        // Assert
        // Check if the merged PDF file is created
        var outputFile = Path.Combine(inputDirPath, outputFileName);
        File.Exists(outputFile).Should().BeTrue();

        // Check if the merged PDF contains the correct number of pages
        using (var mergedPdf = new PdfDocument(new PdfReader(outputFile)))
        {
            mergedPdf.GetNumberOfPages().Should().Be(3);
        }

        // Clean up
        Directory.Delete(inputDirPath, true);
    }
}
