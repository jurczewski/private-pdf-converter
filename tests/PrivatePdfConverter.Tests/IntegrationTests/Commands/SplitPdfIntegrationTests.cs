using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using PrivatePdfConverter.Commands;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class SplitPdfIntegrationTests : IDisposable
{
    private readonly string _testDirectoryPath;
    private readonly string _samplePdfPath;
    private const int SamplePdfPageCount = 5;

    public SplitPdfIntegrationTests()
    {
        _testDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectoryPath);
        _samplePdfPath = Path.Combine(_testDirectoryPath, "sample.pdf");
        CreateSamplePdf(_samplePdfPath);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectoryPath))
        {
            Directory.Delete(_testDirectoryPath, true);
        }
    }

    [Fact]
    public void SplitPdfByPages_ShouldCreateSinglePageFiles_WhenSplittingIndividualPages()
    {
        // Arrange
        const string pages = "1,3,5";
        const string outputBaseName = "page";

        // Act
        SplitPdf.SplitPdfByPages(_samplePdfPath, pages, outputBaseName);

        // Assert
        var expectedFiles = new[]
        {
            Path.Combine(_testDirectoryPath, "page_1.pdf"),
            Path.Combine(_testDirectoryPath, "page_3.pdf"),
            Path.Combine(_testDirectoryPath, "page_5.pdf")
        };

        foreach (var expectedFile in expectedFiles)
        {
            File.Exists(expectedFile).Should().BeTrue();

            // Verify each split PDF has exactly 1 page
            using var splitPdf = new PdfDocument(new PdfReader(expectedFile));
            splitPdf.GetNumberOfPages().Should().Be(1);
        }
    }

    [Fact]
    public void SplitPdfByPages_ShouldCreateRangeFiles_WhenSplittingPageRanges()
    {
        // Arrange
        const string pages = "1-2,4-5";
        const string outputBaseName = "range";

        // Act
        SplitPdf.SplitPdfByPages(_samplePdfPath, pages, outputBaseName);

        // Assert
        var expectedFiles = new Dictionary<string, int>
        {
            { Path.Combine(_testDirectoryPath, "range_1-2.pdf"), 2 },
            { Path.Combine(_testDirectoryPath, "range_4-5.pdf"), 2 }
        };

        foreach (var (expectedFile, expectedPageCount) in expectedFiles)
        {
            File.Exists(expectedFile).Should().BeTrue();

            // Verify each split PDF has the correct number of pages
            using var splitPdf = new PdfDocument(new PdfReader(expectedFile));
            splitPdf.GetNumberOfPages().Should().Be(expectedPageCount);
        }
    }

    [Fact]
    public void SplitPdfByPages_ShouldCreateAllIndividualPages_WhenUsingAllKeyword()
    {
        // Arrange
        const string pages = "all";
        const string outputBaseName = "single";

        // Act
        SplitPdf.SplitPdfByPages(_samplePdfPath, pages, outputBaseName);

        // Assert
        for (int i = 1; i <= SamplePdfPageCount; i++)
        {
            var expectedFile = Path.Combine(_testDirectoryPath, $"single_{i}.pdf");
            File.Exists(expectedFile).Should().BeTrue();

            // Verify each split PDF has exactly 1 page
            using var splitPdf = new PdfDocument(new PdfReader(expectedFile));
            splitPdf.GetNumberOfPages().Should().Be(1);
        }
    }

    [Fact]
    public void SplitPdfByPages_ShouldUseDefaultOutput_WhenOutputIsNotSpecified()
    {
        // Arrange
        const string pages = "1";
        
        // Act
        SplitPdf.SplitPdfByPages(_samplePdfPath, pages, null);

        // Assert
        var expectedFile = Path.Combine(_testDirectoryPath, "output_1.pdf");
        File.Exists(expectedFile).Should().BeTrue();

        using var splitPdf = new PdfDocument(new PdfReader(expectedFile));
        splitPdf.GetNumberOfPages().Should().Be(1);
    }

    [Fact]
    public void SplitPdfByPages_ShouldHandleMixedFormat_WhenCombiningRangesAndIndividualPages()
    {
        // Arrange
        const string pages = "1-2,4,5";
        const string outputBaseName = "mixed";

        // Act
        SplitPdf.SplitPdfByPages(_samplePdfPath, pages, outputBaseName);

        // Assert
        var expectedFiles = new Dictionary<string, int>
        {
            { Path.Combine(_testDirectoryPath, "mixed_1-2.pdf"), 2 },
            { Path.Combine(_testDirectoryPath, "mixed_4.pdf"), 1 },
            { Path.Combine(_testDirectoryPath, "mixed_5.pdf"), 1 }
        };

        foreach (var (expectedFile, expectedPageCount) in expectedFiles)
        {
            File.Exists(expectedFile).Should().BeTrue();

            using var splitPdf = new PdfDocument(new PdfReader(expectedFile));
            splitPdf.GetNumberOfPages().Should().Be(expectedPageCount);
        }
    }

    [Fact]
    public void SplitPdfByPages_ShouldNotCreateFiles_WhenPageRangeIsInvalid()
    {
        // Arrange
        const string pages = "10-15"; // Invalid range for 5-page PDF
        const string outputBaseName = "invalid";

        // Act
        SplitPdf.SplitPdfByPages(_samplePdfPath, pages, outputBaseName);

        // Assert
        var files = Directory.GetFiles(_testDirectoryPath, "invalid*.pdf");
        files.Should().BeEmpty();
    }

    [Fact]
    public void SplitPdfByPages_ShouldNotCreateFiles_WhenFileDoesNotExist()
    {
        // Arrange
        var nonExistentFile = Path.Combine(_testDirectoryPath, "nonexistent.pdf");
        const string pages = "1";
        const string outputBaseName = "test";

        // Act
        SplitPdf.SplitPdfByPages(nonExistentFile, pages, outputBaseName);

        // Assert
        var files = Directory.GetFiles(_testDirectoryPath, "test*.pdf");
        files.Should().BeEmpty();
    }

    [Fact]
    public void SplitPdfByPages_ShouldNotCreateFiles_WhenFileIsNotPdf()
    {
        // Arrange
        var textFile = Path.Combine(_testDirectoryPath, "test.txt");
        File.WriteAllText(textFile, "This is not a PDF");
        const string pages = "1";
        const string outputBaseName = "test";

        // Act
        SplitPdf.SplitPdfByPages(textFile, pages, outputBaseName);

        // Assert
        var files = Directory.GetFiles(_testDirectoryPath, "test*.pdf");
        files.Should().BeEmpty();
    }

    private static void CreateSamplePdf(string filePath)
    {
        using var pdfWriter = new PdfWriter(filePath);
        using var pdfDocument = new PdfDocument(pdfWriter);
        var document = new Document(pdfDocument);

        for (int i = 1; i <= SamplePdfPageCount; i++)
        {
            document.Add(new Paragraph($"This is page {i} of the sample PDF."));
            document.Add(new Paragraph($"Content for testing the split functionality."));
            document.Add(new Paragraph($"Page number: {i}"));
            
            if (i < SamplePdfPageCount)
            {
                document.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.NEXT_PAGE));
            }
        }
    }
}