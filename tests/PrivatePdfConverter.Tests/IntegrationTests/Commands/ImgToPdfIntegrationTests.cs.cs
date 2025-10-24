using System.Diagnostics;
using ImageMagick;
using PrivatePdfConverter.Commands;

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

    [Fact]
    public void ConvertImageToOnePdf_IntegrationTest_ShouldRunExecutableAndCreatePdf()
    {
        // Arrange
        var fixture = new Fixture();
        var inputFileName = fixture.Create<string>() + ".png";
        var inputFilePath = Path.Combine(Path.GetTempPath(), inputFileName);
        var outputFileName = fixture.Create<string>() + ".pdf";

        // Create a dummy image file
        using var image = new MagickImage(MagickColors.Red, 100, 100);
        image.Write(inputFilePath);

        // Act - Run the actual console application as a separate process
        var processInfo = new ProcessStartInfo
        {
            FileName = GetExecutablePath(),
            Arguments = $"img --path \"{inputFilePath}\" --output \"{outputFileName}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(processInfo);
        process.Should().NotBeNull("Process should start successfully");
        process.WaitForExit();

        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();

        // Assert
        // Check if the process completed successfully
        process.ExitCode.Should().Be(0, $"The application should exit successfully. STDOUT: {stdout}, STDERR: {stderr}");

        // Check if the PDF file is created
        var outputPdfPath = Path.Combine(Path.GetDirectoryName(inputFilePath) ?? string.Empty, outputFileName);
        File.Exists(outputPdfPath).Should().BeTrue("The PDF file should be created");

        // Verify the PDF file has content (not empty)
        var fileInfo = new FileInfo(outputPdfPath);
        fileInfo.Length.Should().BeGreaterThan(0, "The PDF file should not be empty");

        // Clean up
        File.Delete(inputFilePath);
        File.Delete(outputPdfPath);
    }

    private static string GetExecutablePath()
    {
        // Get the path to the built executable
        var testAssemblyLocation = typeof(ImgToPdfUnitTests).Assembly.Location;
        var testProjectDir = Path.GetDirectoryName(testAssemblyLocation);

        // Navigate to the main project's build output
        var mainProjectDir = Path.GetFullPath(Path.Combine(testProjectDir!, "..", "..", "..", "..", "..", "src", "PrivatePdfConverter"));
        var executablePath = Path.Combine(mainProjectDir, "bin", "Debug", "net8.0", "PrivatePdfConverter.exe");

        if (!File.Exists(executablePath))
        {
            throw new FileNotFoundException($"Executable not found at: {executablePath}. Make sure the main project is built.");
        }

        return executablePath;
    }
}
