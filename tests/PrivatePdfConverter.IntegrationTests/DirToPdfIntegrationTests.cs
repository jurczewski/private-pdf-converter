using AutoFixture;
using FluentAssertions;
using ImageMagick;
using PrivatePdfConverter.Commands;
using Xunit;

namespace PrivatePdfConverter.UnitTests;

public class DirToPdfIntegrationUnitTests
{
    [Fact]
    public void ConvertDirectoryToOnePdf_ShouldBehaveCorrectly()
    {
        // Arrange
        var fixture = new Fixture();
        var inputDirectoryName = fixture.Create<string>();
        var inputDirPath = Path.Combine(Path.GetTempPath(), inputDirectoryName);
        const string outputFileName = "final.pdf";
        Directory.CreateDirectory(inputDirPath);

        // Create dummy image files
        var imagePaths = new[]
        {
            Path.Combine(inputDirPath, "image1.png"),
            Path.Combine(inputDirPath, "image2.jpg"),
            Path.Combine(inputDirPath, "image3.bmp")
        };

        foreach (var imagePath in imagePaths)
        {
            using var image = new MagickImage(MagickColors.Red, 100, 100);
            image.Write(imagePath);
        }

        // Act
        DirToPdf.ConvertDirectoryToOnePdf(inputDirPath, outputFileName);

        // Assert
        // Check if there is any pdf file created
        var outputPdfPath = Directory.GetFiles(inputDirPath, "*.pdf").FirstOrDefault();
        File.Exists(outputPdfPath).Should().BeTrue();

        // Check if there is a specif pdf with that name
        var output = Path.Combine(Path.GetTempPath(), $"{inputDirectoryName}/{outputFileName}");
        File.Exists(output).Should().BeTrue();

        // Clean up
        Directory.Delete(inputDirPath, true);
    }
}
