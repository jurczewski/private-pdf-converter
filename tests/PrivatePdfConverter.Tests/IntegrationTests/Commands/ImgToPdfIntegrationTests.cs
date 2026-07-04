using ImageMagick;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class ImgToPdfIntegrationTests
{
    [Fact]
    public void ShouldCreatePdf()
    {
        var inputFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
        var outputName = Guid.NewGuid().ToString("N");
        var outputPdfPath = Path.Combine(Path.GetTempPath(), outputName + ".pdf");

        try
        {
            using (var image = new MagickImage(MagickColors.Red, 100, 100))
                image.Write(inputFilePath);

            var result = CliTestHelper.Run("img", "--path", inputFilePath, "--output", outputName);

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(outputPdfPath).Should().BeTrue();
        }
        finally
        {
            if (File.Exists(inputFilePath)) File.Delete(inputFilePath);
            if (File.Exists(outputPdfPath)) File.Delete(outputPdfPath);
        }
    }

    [Fact]
    public void ShouldCreatePdf_WhenOutputIsOmitted()
    {
        var inputFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
        var expectedOutputPath = Path.ChangeExtension(inputFilePath, ".pdf");

        try
        {
            using (var image = new MagickImage(MagickColors.Red, 100, 100))
                image.Write(inputFilePath);

            var result = CliTestHelper.Run("img", "--path", inputFilePath);

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(expectedOutputPath).Should().BeTrue();
        }
        finally
        {
            if (File.Exists(inputFilePath)) File.Delete(inputFilePath);
            if (File.Exists(expectedOutputPath)) File.Delete(expectedOutputPath);
        }
    }

    [Fact]
    public void ShouldNotCreatePdf_WhenImageExceedsLimits()
    {
        var inputFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
        var expectedOutputPath = Path.ChangeExtension(inputFilePath, ".pdf");

        try
        {
            using (var image = new MagickImage(MagickColors.Red, 10_001, 100))
                image.Write(inputFilePath);

            var result = CliTestHelper.Run("img", "--path", inputFilePath);

            result.ExitCode.Should().Be(0, $"stderr: {result.StandardError}\nstdout: {result.StandardOutput}");
            File.Exists(expectedOutputPath).Should().BeFalse();
        }
        finally
        {
            if (File.Exists(inputFilePath)) File.Delete(inputFilePath);
            if (File.Exists(expectedOutputPath)) File.Delete(expectedOutputPath);
        }
    }
}
