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
    public void PathCombine_WithPath_ShouldCombine()
    {
        // Arrange
        var path = $"C:{Path.DirectorySeparatorChar}dir";
        const string fileName = "output.pdf";

        // Act
        var result = Path.Combine(path, fileName);

        // Assert
        result.Should().Be($"C:{Path.DirectorySeparatorChar}dir{Path.DirectorySeparatorChar}output.pdf");
    }

    [Fact]
    public void PathCombine_WithNullPath_ShouldReturnFileName()
    {
        // Arrange
        string? path = null;
        const string fileName = "output.pdf";

        // Act
        var result = Path.Combine(path ?? string.Empty, fileName);

        // Assert
        result.Should().Be("output.pdf");
    }
}
