using ImageMagick;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class DirToPdfIntegrationTests
{
    [Fact]
    public void ShouldCreatePdf_FromImagesInDirectory()
    {
        var inputDirPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(inputDirPath);
        const string outputName = "final";
        var outputPdfPath = Path.Combine(inputDirPath, outputName + ".pdf");

        try
        {
            foreach (var (name, color) in new[] { ("a.png", MagickColors.Red), ("b.jpg", MagickColors.Blue), ("c.bmp", MagickColors.Green) })
            {
                using var image = new MagickImage(color, 100, 100);
                image.Write(Path.Combine(inputDirPath, name));
            }

            var result = CliTestHelper.Run("dir", "--path", inputDirPath, "--output", outputName);

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(outputPdfPath).Should().BeTrue();
        }
        finally
        {
            if (Directory.Exists(inputDirPath)) Directory.Delete(inputDirPath, true);
        }
    }

    [Fact]
    public void ShouldNotCreatePdf_WhenImageExceedsLimits()
    {
        var inputDirPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(inputDirPath);
        const string outputName = "final";
        var outputPdfPath = Path.Combine(inputDirPath, outputName + ".pdf");

        try
        {
            using (var image = new MagickImage(MagickColors.Red, 10_001, 100))
                image.Write(Path.Combine(inputDirPath, "oversized.png"));

            var result = CliTestHelper.Run("dir", "--path", inputDirPath, "--output", outputName);

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(outputPdfPath).Should().BeFalse();
        }
        finally
        {
            if (Directory.Exists(inputDirPath)) Directory.Delete(inputDirPath, true);
        }
    }
}
