using CourseCrutch.Infrastructure.Ocr;

namespace CourseCrutch.Tests.Infrastructure;

public sealed class MockOcrAnalysisServiceTests
{
    private readonly MockOcrAnalysisService _service = new();

    [Theory]
    [InlineData("", 0, 0)]
    [InlineData("one two three", 3, 13)]
    [InlineData("  spaced   out  ", 2, 16)]
    public async Task AnalyseAsync_AnyText_CountsWordsAndCharacters(string text, int words, int characters)
    {
        var result = await _service.AnalyseAsync(text);

        Assert.Equal(words, result.WordCount);
        Assert.Equal(characters, result.CharacterCount);
    }

    [Fact]
    public async Task AnalyseAsync_ShortWordsAndPunctuation_KeepsTrimmedWordsLongerThanThree()
    {
        var result = await _service.AnalyseAsync("The quick, brown fox jumps!");

        Assert.Equal(["quick", "brown", "jumps"], result.Keywords);
    }

    [Fact]
    public async Task AnalyseAsync_RepeatedWordsInDifferentCase_ReturnsDistinctKeywords()
    {
        var result = await _service.AnalyseAsync("Exam exam EXAM notes");

        Assert.Equal(["Exam", "notes"], result.Keywords);
    }

    [Fact]
    public async Task AnalyseAsync_ManyWords_ReturnsAtMostFiveKeywords()
    {
        var result = await _service.AnalyseAsync("alpha bravo charlie delta echoes foxtrot golfer");

        Assert.Equal(5, result.Keywords.Count);
    }

    [Fact]
    public async Task AnalyseAsync_AnyText_ReturnsMockedAnswer()
    {
        var result = await _service.AnalyseAsync("anything");

        Assert.Equal("Mocked answer. No real analysis has run.", result.Answer);
    }
}
