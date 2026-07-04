using ImageMagick;
using PrivatePdfConverter.Services;

namespace PrivatePdfConverter.Tests.UnitTests;

public sealed class FileServiceUnitTests
{
    [Theory]
    [InlineData(".jpg", true)]
    [InlineData(".jpeg", true)]
    [InlineData(".png", true)]
    [InlineData(".bmp", true)]
    [InlineData(".gif", true)]
    [InlineData(".tif", true)]
    [InlineData(".tiff", true)]
    [InlineData(".webp", true)]
    [InlineData(".JPG", true)]
    [InlineData(".txt", false)]
    [InlineData(".pdf", false)]
    [InlineData("", false)]
    [InlineData(".unknown", false)]
    [InlineData(null, false)]
    public void IsImage_ShouldReturnCorrectResult(string? extension, bool expected)
    {
        // Act
        var result = extension.IsImage();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void PrepareOutputFileName_WithOutputProvided_ShouldReturnOutputWithPdfExtension()
    {
        // Arrange
        const string output = "test";
        const string sourcePath = "dummy.pdf";

        // Act
        var result = output.PrepareOutputFileName(sourcePath);

        // Assert
        result.Should().Be("test.pdf");
    }

    [Fact]
    public void PrepareOutputFileName_WithOutputPdf_ShouldReturnAsIs()
    {
        // Arrange
        const string output = "test.pdf";
        const string sourcePath = "dummy.jpg";

        // Act
        var result = output.PrepareOutputFileName(sourcePath);

        // Assert
        result.Should().Be("test.pdf");
    }

    [Fact]
    public void PrepareOutputFileName_WithNullOutput_ShouldDeriveFromSourcePath_WhenPathHasTrailingSlash()
    {
        // Arrange
        string? output = null;
        var sourcePath = Path.Combine("images", "my-photos") + Path.DirectorySeparatorChar;

        // Act
        var result = output.PrepareOutputFileName(sourcePath);

        // Assert
        result.Should().Be("my-photos.pdf");
    }

    [Fact]
    public void PrepareOutputFileName_WithNullOutput_ShouldAppendExportSuffix_WhenSourceIsPdf()
    {
        // Arrange
        string? output = null;
        const string sourcePath = "report.pdf";

        // Act
        var result = output.PrepareOutputFileName(sourcePath);

        // Assert
        result.Should().Be("report_export.pdf");
    }

    [Fact]
    public void PrepareOutputFileName_WithNullOutput_ShouldDeriveFromSourcePath()
    {
        // Arrange
        string? output = null;
        var sourcePath = Path.Combine("images", "photo.jpg");

        // Act
        var result = output.PrepareOutputFileName(sourcePath);

        // Assert
        result.Should().Be("photo.pdf");
    }

    [Fact]
    public void PrepareOutputFileName_WithEmptyOutput_ShouldDeriveFromSourcePath()
    {
        // Arrange
        const string output = "";
        var sourcePath = Path.Combine("path", "to", "file.png");

        // Act
        var result = output.PrepareOutputFileName(sourcePath);

        // Assert
        result.Should().Be("file.pdf");
    }

    [Fact]
    public void LoadValidatedImage_ShouldReturnImage_WhenWithinLimits()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");

        try
        {
            using (var image = new MagickImage(MagickColors.Red, 100, 100))
            {
                image.Write(path);
            }

            using var result = FileService.LoadValidatedImage(path);

            result.Should().NotBeNull();
            result.Width.Should().Be(100);
            result.Height.Should().Be(100);
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void LoadValidatedImage_ShouldReturnNull_WhenWidthExceedsLimit()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");

        try
        {
            using (var image = new MagickImage(MagickColors.Red, 10_001, 100))
            {
                image.Write(path);
            }

            FileService.LoadValidatedImage(path).Should().BeNull();
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void LoadValidatedImage_ShouldReturnNull_WhenAreaExceedsLimit()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");

        try
        {
            // Within per-side limits (10_000) but 5000 * 20_001 > 100_000_000 total area.
            using (var image = new MagickImage(MagickColors.Red, 5000, 20_001))
            {
                image.Write(path);
            }

            FileService.LoadValidatedImage(path).Should().BeNull();
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
