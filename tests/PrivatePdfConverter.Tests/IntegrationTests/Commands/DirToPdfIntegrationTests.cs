using ImageMagick;
using PrivatePdfConverter.Commands;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class DirToPdfIntegrationUnitTests
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

    [Fact]
    public void ConvertDirectoryToOnePdf_ShouldNotCreatePdf_WhenImageExceedsLimits()
    {
        // End-to-end smoke: dir command must not write a PDF when LoadValidatedImage rejects input.
        var inputDirPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        const string outputFileName = "final.pdf";
        var outputPath = Path.Combine(inputDirPath, outputFileName);
        Directory.CreateDirectory(inputDirPath);

        var oversizedImagePath = Path.Combine(inputDirPath, "oversized.png");

        try
        {
            using (var image = new MagickImage(MagickColors.Red, 10_001, 100))
            {
                image.Write(oversizedImagePath);
            }

            DirToPdf.ConvertDirectoryToOnePdf(inputDirPath, outputFileName);

            File.Exists(outputPath).Should().BeFalse();
        }
        finally
        {
            if (Directory.Exists(inputDirPath))
            {
                Directory.Delete(inputDirPath, true);
            }
        }
    }
}
