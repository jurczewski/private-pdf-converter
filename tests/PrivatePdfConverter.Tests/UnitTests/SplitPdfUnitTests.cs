using PrivatePdfConverter.Commands;

namespace PrivatePdfConverter.Tests.UnitTests;

public sealed class SplitPdfUnitTests
{
    [Fact]
    public void ParsePageRanges_ShouldParseSingleRange()
    {
        var result = SplitPdf.ParsePageRanges("1-5", totalPages: 10);

        result.Should().ContainSingle();
        result![0].Should().Be((1, 5, "1-5"));
    }

    [Fact]
    public void ParsePageRanges_ShouldParseIndividualPages()
    {
        var result = SplitPdf.ParsePageRanges("1,3,5", totalPages: 10);

        result.Should().Equal((1, 1, "1"), (3, 3, "3"), (5, 5, "5"));
    }

    [Fact]
    public void ParsePageRanges_ShouldParseMixedFormat()
    {
        var result = SplitPdf.ParsePageRanges("1-3,5,8-10", totalPages: 10);

        result.Should().Equal((1, 3, "1-3"), (5, 5, "5"), (8, 10, "8-10"));
    }

    [Fact]
    public void ParsePageRanges_ShouldExpandAllKeyword()
    {
        var result = SplitPdf.ParsePageRanges("all", totalPages: 3);

        result.Should().Equal((1, 1, "1"), (2, 2, "2"), (3, 3, "3"));
    }

    [Fact]
    public void ParsePageRanges_ShouldBeCaseInsensitiveForAll()
    {
        var result = SplitPdf.ParsePageRanges("ALL", totalPages: 2);

        result.Should().Equal((1, 1, "1"), (2, 2, "2"));
    }

    [Fact]
    public void ParsePageRanges_ShouldIgnoreWhitespace_AndNormalizeLabels()
    {
        var result = SplitPdf.ParsePageRanges(" 1 - 3 , 5 ", totalPages: 10);

        result.Should().Equal((1, 3, "1-3"), (5, 5, "5"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("abc")]
    [InlineData("1-abc")]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("5-1")]
    [InlineData("1-100")]
    [InlineData("100")]
    public void ParsePageRanges_ShouldReturnNull_ForInvalidInput(string pages)
    {
        var result = SplitPdf.ParsePageRanges(pages, totalPages: 10);

        result.Should().BeNull();
    }
}
