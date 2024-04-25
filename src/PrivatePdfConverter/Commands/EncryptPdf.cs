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

        var outputFileName = output.PrepareOutputFileName();
        var exportFullPath = path.AddFileToPath(outputFileName);

        EncryptPdfFile(path, password, outputFileName);

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, exportFullPath);
    }

    private static void EncryptPdfFile(string path, string password, string outputFileName)
    {
        var passwordBytes = Encoding.Default.GetBytes(password);

        using var pdfReader = new PdfReader(path);
        var writerProperties = new WriterProperties()
            .SetStandardEncryption(
                passwordBytes,
                passwordBytes,
                EncryptionConstants.ALLOW_PRINTING,
                EncryptionConstants.ENCRYPTION_AES_128);
        using var pdfWriter = new PdfWriter(new FileStream(outputFileName, FileMode.Create), writerProperties);
        using var pdfDocument = new PdfDocument(pdfReader, pdfWriter);

        pdfDocument.Close();
    }
}
