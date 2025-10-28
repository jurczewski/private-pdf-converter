using System.Text;
using iText.Kernel.Pdf;
using PrivatePdfConverter.Services;

namespace PrivatePdfConverter.Commands;

public static class EncryptPdf
{
    public static void EncryptPdfWithPassword(string path, string password, string? output)
    {
        PdfOperationHelper.LogSingleFileRead(path);
        var exportFullPath = PdfOperationHelper.PrepareOutputPath(path, output, out var outputFileName);

        EncryptPdfFile(path, password, exportFullPath);

        PdfOperationHelper.LogPdfCreation(outputFileName, exportFullPath);
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
