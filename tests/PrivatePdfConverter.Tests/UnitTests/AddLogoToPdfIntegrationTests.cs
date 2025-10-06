using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using PrivatePdfConverter.Commands;

namespace PrivatePdfConverter.Tests.UnitTests;

public sealed class AddLogoToPdfTests
{
    [Theory]
    [InlineData("top-left")]
    [InlineData("top-right")]
    [InlineData("bottom-left")]
    [InlineData("bottom-right")]
    public void CalculatePosition_ShouldBehaveCorrectly(string position)
    {
        // Arrange
        var filename = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");
        using (var pdfDoc = new PdfDocument(new PdfWriter(filename)))
        {
            var document = new iText.Layout.Document(pdfDoc);
            document.Add(new Paragraph("This is a sample PDF."));
            var logo = new Image(ImageDataFactory.Create("../../../../../assets/logo.jpeg"));
            // Act
            var (x, y) = AddLogoToPdf.CalculatePosition(pdfDoc, position, logo);

            // Assert
            switch (position)
            {
                case "top-left":
                    x.Should().Be(0);
                    y.Should().NotBe(0);
                    break;
                case "top-right":
                    x.Should().NotBe(0);
                    y.Should().NotBe(0);
                    break;
                case "bottom-left":
                    x.Should().Be(0);
                    y.Should().Be(0);
                    break;
                case "bottom-right":
                    x.Should().NotBe(0);
                    y.Should().Be(0);
                    break;
            }
        }

        if (File.Exists(filename))
        {
            File.Delete(filename);
        }
    }
}
