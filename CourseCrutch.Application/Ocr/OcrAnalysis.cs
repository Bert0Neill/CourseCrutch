namespace CourseCrutch.Application.Ocr;

public sealed record OcrAnalysis(int WordCount, int CharacterCount, string Summary, IReadOnlyList<string> Keywords);
