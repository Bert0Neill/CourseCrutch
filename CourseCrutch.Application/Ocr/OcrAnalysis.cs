namespace CourseCrutch.Application.Ocr;

public sealed record OcrAnalysis(int WordCount, int CharacterCount, string Answer, IReadOnlyList<string> Keywords);
