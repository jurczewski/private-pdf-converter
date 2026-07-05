using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class SplitPdfIntegrationTests
{
    [Fact]
    public void ShouldSplitByPageRanges()
    {
        var inputDirPath = CreateTempDir();
        var inputPdfPath = Path.Combine(inputDirPath, "input.pdf");

        try
        {
            CreateSamplePdf(inputPdfPath, 15);

            var result = CliTestHelper.Run("split", "--path", inputPdfPath, "--pages", "1-5,10-15", "--output", "part");

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");

            var firstPart = Path.Combine(inputDirPath, "part_1-5.pdf");
            var secondPart = Path.Combine(inputDirPath, "part_10-15.pdf");
            File.Exists(firstPart).Should().BeTrue();
            File.Exists(secondPart).Should().BeTrue();

            GetPageCount(firstPart).Should().Be(5);
            GetPageCount(secondPart).Should().Be(6);
        }
        finally
        {
            Cleanup(inputDirPath);
        }
    }

    [Fact]
    public void ShouldSplitIndividualPages()
    {
        var inputDirPath = CreateTempDir();
        var inputPdfPath = Path.Combine(inputDirPath, "input.pdf");

        try
        {
            CreateSamplePdf(inputPdfPath, 5);

            var result = CliTestHelper.Run("split", "--path", inputPdfPath, "--pages", "1,3,5", "--output", "page");

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");

            foreach (var page in new[] { 1, 3, 5 })
            {
                var partPath = Path.Combine(inputDirPath, $"page_{page}.pdf");
                File.Exists(partPath).Should().BeTrue();
                GetPageCount(partPath).Should().Be(1);
            }
        }
        finally
        {
            Cleanup(inputDirPath);
        }
    }

    [Fact]
    public void ShouldSplitMixedFormat()
    {
        var inputDirPath = CreateTempDir();
        var inputPdfPath = Path.Combine(inputDirPath, "input.pdf");

        try
        {
            CreateSamplePdf(inputPdfPath, 10);

            var result = CliTestHelper.Run("split", "--path", inputPdfPath, "--pages", "1-3,5,8-10", "--output", "mix");

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");

            GetPageCount(Path.Combine(inputDirPath, "mix_1-3.pdf")).Should().Be(3);
            GetPageCount(Path.Combine(inputDirPath, "mix_5.pdf")).Should().Be(1);
            GetPageCount(Path.Combine(inputDirPath, "mix_8-10.pdf")).Should().Be(3);
        }
        finally
        {
            Cleanup(inputDirPath);
        }
    }

    [Fact]
    public void ShouldSplitAllPages()
    {
        var inputDirPath = CreateTempDir();
        var inputPdfPath = Path.Combine(inputDirPath, "input.pdf");

        try
        {
            CreateSamplePdf(inputPdfPath, 4);

            var result = CliTestHelper.Run("split", "--path", inputPdfPath, "--pages", "all", "--output", "single");

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");

            for (var page = 1; page <= 4; page++)
            {
                var partPath = Path.Combine(inputDirPath, $"single_{page}.pdf");
                File.Exists(partPath).Should().BeTrue();
                GetPageCount(partPath).Should().Be(1);
            }
        }
        finally
        {
            Cleanup(inputDirPath);
        }
    }

    [Fact]
    public void ShouldUseDefaultOutputName_WhenOutputOmitted()
    {
        var inputDirPath = CreateTempDir();
        var inputPdfPath = Path.Combine(inputDirPath, "input.pdf");

        try
        {
            CreateSamplePdf(inputPdfPath, 5);

            var result = CliTestHelper.Run("split", "--path", inputPdfPath, "--pages", "1-2");

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(Path.Combine(inputDirPath, "input_pages_1-2.pdf")).Should().BeTrue();
        }
        finally
        {
            Cleanup(inputDirPath);
        }
    }

    [Fact]
    public void ShouldNormalizeOutput_WhenOutputEndsWithPdfExtension()
    {
        var inputDirPath = CreateTempDir();
        var inputPdfPath = Path.Combine(inputDirPath, "input.pdf");

        try
        {
            CreateSamplePdf(inputPdfPath, 5);

            var result = CliTestHelper.Run("split", "--path", inputPdfPath, "--pages", "1-2", "--output", "part.pdf");

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(Path.Combine(inputDirPath, "part_1-2.pdf")).Should().BeTrue();
            File.Exists(Path.Combine(inputDirPath, "part.pdf_1-2.pdf")).Should().BeFalse();
        }
        finally
        {
            Cleanup(inputDirPath);
        }
    }

    [Fact]
    public void ShouldNotCreateOutput_WhenRangeExceedsPageCount()
    {
        var inputDirPath = CreateTempDir();
        var inputPdfPath = Path.Combine(inputDirPath, "input.pdf");

        try
        {
            CreateSamplePdf(inputPdfPath, 3);

            var result = CliTestHelper.Run("split", "--path", inputPdfPath, "--pages", "1-10", "--output", "part");

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(Path.Combine(inputDirPath, "part_1-10.pdf")).Should().BeFalse();
        }
        finally
        {
            Cleanup(inputDirPath);
        }
    }

    [Fact]
    public void ShouldNotCreateOutput_WhenFileDoesNotExist()
    {
        var inputDirPath = CreateTempDir();
        var missingPdfPath = Path.Combine(inputDirPath, "does-not-exist.pdf");

        try
        {
            var result = CliTestHelper.Run("split", "--path", missingPdfPath, "--pages", "1", "--output", "part");

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(Path.Combine(inputDirPath, "part_1.pdf")).Should().BeFalse();
        }
        finally
        {
            Cleanup(inputDirPath);
        }
    }

    [Fact]
    public void ShouldNotCreateOutput_WhenFileIsNotPdf()
    {
        var inputDirPath = CreateTempDir();
        var txtPath = Path.Combine(inputDirPath, "input.txt");

        try
        {
            File.WriteAllText(txtPath, "not a pdf");

            var result = CliTestHelper.Run("split", "--path", txtPath, "--pages", "1", "--output", "part");

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(Path.Combine(inputDirPath, "part_1.pdf")).Should().BeFalse();
        }
        finally
        {
            Cleanup(inputDirPath);
        }
    }

    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    private static void CreateSamplePdf(string filePath, int pageCount)
    {
        using var writer = new PdfWriter(filePath);
        using var pdf = new PdfDocument(writer);
        var doc = new Document(pdf);
        for (var page = 1; page <= pageCount; page++)
        {
            doc.Add(new Paragraph($"Page {page}"));
            if (page < pageCount)
            {
                doc.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.NEXT_PAGE));
            }
        }
    }

    private static int GetPageCount(string filePath)
    {
        using var pdf = new PdfDocument(new PdfReader(filePath));
        return pdf.GetNumberOfPages();
    }

    private static void Cleanup(string dir)
    {
        if (Directory.Exists(dir)) Directory.Delete(dir, true);
    }
}
