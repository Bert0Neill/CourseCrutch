using Microsoft.Maui.Media;

namespace CourseCrutch.Clients.Services;

public sealed class PhotoCaptureService : IPhotoCaptureService
{
    public async Task<byte[]?> CapturePhotoAsync()
    {
        if (!MediaPicker.Default.IsCaptureSupported)
        {
            return null;
        }

        var photo = await MediaPicker.Default.CapturePhotoAsync();
        if (photo is null)
        {
            return null;
        }

        try
        {
            using var stream = await photo.OpenReadAsync();
            using var memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            return memory.ToArray();
        }
        finally
        {
            if (File.Exists(photo.FullPath))
            {
                File.Delete(photo.FullPath);
            }
        }
    }
}
