using MemoryPlaces.Application.Image;

namespace MemoryPlaces.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadBlobAsync(ImageDto image);
    Task DeleteBlobAsync(string blobName);
}
