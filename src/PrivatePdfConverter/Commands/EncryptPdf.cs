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
        var exportFullPath = $"{path}/{outputFileName}";

        var passwordBytes = Encoding.Default.GetBytes(password);

        var pdfReader = new PdfReader(path);
        var writerProperties = new WriterProperties()
            .SetStandardEncryption(
                passwordBytes,
                passwordBytes,
                EncryptionConstants.ALLOW_PRINTING,
                EncryptionConstants.ENCRYPTION_AES_128);
        var pdfWriter = new PdfWriter(new FileStream(outputFileName, FileMode.Create), writerProperties);
        var pdfDocument = new PdfDocument(pdfReader, pdfWriter);
        pdfDocument.Close();

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, exportFullPath);
    }
}
