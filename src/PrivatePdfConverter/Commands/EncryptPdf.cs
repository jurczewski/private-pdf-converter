using System.Text;
using iText.Kernel.Pdf;
using PrivatePdfConverter.Services;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class EncryptPdf
{
    public static void EncryptPdfWithPassword(string path, string password, string? output)
    {
        Log.Logger.Information("Read 1 file with name: {FileName}, Full path: '{Path}'", Path.GetFileName(path), path);
        var outputFileName = output.PrepareOutputFileName(path);
        var exportFullPath = Path.GetDirectoryName(path).AddFileToPath(outputFileName);

        EncryptPdfFile(path, password, exportFullPath);

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, exportFullPath);
    }

    private static void EncryptPdfFile(string sourcePath, string password, string exportFullPath)
    {
        var passwordBytes = Encoding.Default.GetBytes(password);

        using var pdfReader = new PdfReader(sourcePath);
        var writerProperties = new WriterProperties()
            .SetStandardEncryption(
                passwordBytes,
                passwordBytes,
                EncryptionConstants.ALLOW_PRINTING,
                EncryptionConstants.ENCRYPTION_AES_128);
        using var pdfWriter = new PdfWriter(new FileStream(exportFullPath, FileMode.Create), writerProperties);
        using var pdfDocument = new PdfDocument(pdfReader, pdfWriter);

        pdfDocument.Close();
    }
}
