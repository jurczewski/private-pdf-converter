using AutoFixture;
using FluentAssertions;
using ImageMagick;
using PrivatePdfConverter.Commands;
using Xunit;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class ImgToPdfUnitTests
{
    [Fact]
    public void ConvertImageToOnePdf_ShouldBehaveCorrectly()
    {
        // Arrange
        var fixture = new Fixture();
        var inputFileName = fixture.Create<string>() + ".png";
        var inputFilePath = Path.Combine(Path.GetTempPath(), inputFileName);
        var outputFileName = fixture.Create<string>() + ".pdf";

        // Create a dummy image file
        using var image = new MagickImage(MagickColors.Red, 100, 100);
        image.Write(inputFilePath);

        // Act
        ImgToPdf.ConvertImageToOnePdf(inputFilePath, outputFileName);

        // Assert
        // Check if the PDF file is created
        var outputPdfPath = Path.Combine(Path.GetDirectoryName(inputFilePath) ?? string.Empty, outputFileName);
        File.Exists(outputPdfPath).Should().BeTrue();

        // Clean up
        File.Delete(inputFilePath);
        File.Delete(outputPdfPath);
    }
}
