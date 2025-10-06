using System.Text;
using iText.Kernel.Pdf;
using PrivatePdfConverter.Commands;

namespace PrivatePdfConverter.Tests.IntegrationTests.Commands;

public sealed class EncryptPdfTests : IDisposable
{
    private const string EncryptedPdfName = "encrypted.pdf";
    private readonly string _samplePdfPath;
    private readonly string _encryptedPdfPath;
    private readonly string _password;

    public EncryptPdfTests()
    {
        var fixture = new Fixture();
        _password = fixture.Create<string>();

        var samplePdfName = fixture.Create<string>();
        _samplePdfPath = Path.Combine(Path.GetTempPath(), samplePdfName);
        _encryptedPdfPath = Path.Combine(Path.GetTempPath(), EncryptedPdfName);
    }

    public void Dispose()
    {
        if (File.Exists(_samplePdfPath))
            File.Delete(_samplePdfPath);

        if (File.Exists(_encryptedPdfPath))
            File.Delete(_encryptedPdfPath);
    }

    [Fact]
    public void EncryptPdfWithPassword_ShouldBehaveCorrectly()
    {
        // Arrange
        CreateSamplePdf(_samplePdfPath);

        // Act
        EncryptPdf.EncryptPdfWithPassword(_samplePdfPath, _password, EncryptedPdfName);

        // Assert
        // Try to open the encrypted PDF with the password
        var openEncryptedPdf = () =>
        {
            using var pdfReader = new PdfReader(_encryptedPdfPath, new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(_password)));
            using var pdfDocument = new PdfDocument(pdfReader);
        };

        openEncryptedPdf.Should().NotThrow("because the encrypted PDF should open with the correct password");
        File.Exists(_encryptedPdfPath).Should().BeTrue("because the encrypted PDF should be created");
    }

    private static void CreateSamplePdf(string filePath)
    {
        using var pdfWriter = new PdfWriter(filePath);
        using var pdfDocument = new PdfDocument(pdfWriter);
        var document = new iText.Layout.Document(pdfDocument);
        document.Add(new iText.Layout.Element.Paragraph("This is a sample PDF."));
    }
}
