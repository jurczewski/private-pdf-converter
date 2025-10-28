using ImageMagick;
using Serilog;

namespace PrivatePdfConverter.Services;

public static class PdfOperationHelper
{
    /// <summary>
    /// Saves a collection of MagickImages as a PDF file and logs the operation
    /// </summary>
    /// <param name="basePath">The base path where the PDF will be saved</param>
    /// <param name="images">Collection of images to save as PDF</param>
    /// <param name="output">Optional output filename (defaults to "output.pdf")</param>
    /// <param name="useDirectoryName">If true, uses the directory of basePath; if false, uses basePath directly as the directory</param>
    public static void SaveImageCollectionAsPdf(string basePath, MagickImageCollection images, string? output, bool useDirectoryName = false)
    {
        var outputFileName = output.PrepareOutputFileName();
        var fileWithPath = useDirectoryName 
            ? Path.GetDirectoryName(basePath).AddFileToPath(outputFileName)
            : basePath.AddFileToPath(outputFileName);

        images.Write(fileWithPath);

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, fileWithPath);
    }

    /// <summary>
    /// Logs the reading of a single file with standardized format
    /// </summary>
    /// <param name="filePath">Path to the file being read</param>
    public static void LogSingleFileRead(string filePath)
    {
        Log.Logger.Information("Read 1 file with name: {FileName}, Full path: '{Path}'", 
            Path.GetFileName(filePath), filePath);
    }

    /// <summary>
    /// Prepares output path for PDF operations and logs file creation
    /// </summary>
    /// <param name="sourcePath">Source file path</param>
    /// <param name="output">Optional output filename</param>
    /// <param name="outputFileName">Returns the prepared output filename</param>
    /// <returns>Full path where the file will be created</returns>
    public static string PrepareOutputPath(string sourcePath, string? output, out string outputFileName)
    {
        outputFileName = output.PrepareOutputFileName();
        return Path.GetDirectoryName(sourcePath).AddFileToPath(outputFileName);
    }

    /// <summary>
    /// Logs PDF creation with standardized format
    /// </summary>
    /// <param name="outputFileName">Name of the created file</param>
    /// <param name="fullPath">Full path where file was created</param>
    public static void LogPdfCreation(string outputFileName, string fullPath)
    {
        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, fullPath);
    }

    /// <summary>
    /// Prepares output path for operations that work with directory paths directly
    /// </summary>
    /// <param name="directoryPath">Directory path where the file will be saved</param>
    /// <param name="output">Optional output filename</param>
    /// <param name="outputFileName">Returns the prepared output filename</param>
    /// <returns>Full path where the file will be created</returns>
    public static string PrepareDirectoryOutputPath(string directoryPath, string? output, out string outputFileName)
    {
        outputFileName = output.PrepareOutputFileName();
        return directoryPath.AddFileToPath(outputFileName);
    }
}