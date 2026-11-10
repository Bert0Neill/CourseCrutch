namespace CourseCrutch.Clients.Services;

public interface IPhotoCaptureService
{
    Task<byte[]?> CapturePhotoAsync();
}
