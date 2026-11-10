namespace CourseCrutch.Clients.Services;

public interface IOcrService
{
    Task<string> RecognizeTextAsync(byte[] image);
}
