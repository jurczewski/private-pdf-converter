using ImageMagick;
using PrivatePdfConverter.Commands;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class ImgToPdfIntegrationTests
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

    [Fact]
    public void ConvertImageToOnePdf_ShouldNotCreatePdf_WhenImageExceedsLimits()
    {
        // End-to-end smoke: command must not write a PDF when LoadValidatedImage rejects input.
        var inputFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
        var expectedOutputPath = Path.ChangeExtension(inputFilePath, ".pdf");

        try
        {
            using (var image = new MagickImage(MagickColors.Red, 10_001, 100))
            {
                image.Write(inputFilePath);
            }

            ImgToPdf.ConvertImageToOnePdf(inputFilePath, null);

            File.Exists(expectedOutputPath).Should().BeFalse();
        }
        finally
        {
            if (File.Exists(inputFilePath))
            {
                File.Delete(inputFilePath);
            }

            if (File.Exists(expectedOutputPath))
            {
                File.Delete(expectedOutputPath);
            }
        }
    }
}
