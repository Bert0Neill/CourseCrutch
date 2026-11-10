using TesseractOcrMaui;
using TesseractOcrMaui.Enums;

namespace CourseCrutch.Clients.Services;

public sealed class OcrService(ITesseract tesseract) : IOcrService
{
    public async Task<string> RecognizeTextAsync(byte[] image)
    {
        var result = await tesseract.RecognizeTextAsync(image);
        if (result.Status.NotSuccess())
        {
            throw new InvalidOperationException($"OCR failed ({result.Status}): {result.Message} {result.Exception?.Message}".Trim());
        }

        return result.RecognisedText ?? string.Empty;
    }
}
