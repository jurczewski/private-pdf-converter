using FluentAssertions;
using ImageMagick;
using PrivatePdfConverter.Commands;
using Xunit;

namespace PrivatePdfConverter.UnitTests
{
    public class DirToPdfIntegrationUnitTests
    {
        [Fact]
        public void ConvertDirectoryToOnePdf_Success()
        {
            // Arrange
            var inputDirectory = Path.Combine(Path.GetTempPath(), "InputDirectory");
            var outputDirectory = Path.Combine(Path.GetTempPath(), "OutputDirectory");
            Directory.CreateDirectory(inputDirectory);
            Directory.CreateDirectory(outputDirectory);

            // Create dummy image files
            var imagePaths = new[]
            {
                Path.Combine(inputDirectory, "image1.png"),
                Path.Combine(inputDirectory, "image2.jpg"),
                Path.Combine(inputDirectory, "image3.bmp")
            };

            foreach (var imagePath in imagePaths)
            {
                using var image = new MagickImage(MagickColors.Red, 100, 100);
                image.Write(imagePath);
            }

            // Act
            DirToPdf.ConvertDirectoryToOnePdf(inputDirectory, outputDirectory);

            // Assert
            var outputPdfPath = Directory.GetFiles(outputDirectory, "*.pdf").FirstOrDefault();
            outputPdfPath.Should().NotBeNull();
            outputPdfPath.Should().BeOfType<string>();
            File.Exists(outputPdfPath).Should().BeTrue();

            // Clean up
            Directory.Delete(inputDirectory, true);
            Directory.Delete(outputDirectory, true);
        }
    }
}
