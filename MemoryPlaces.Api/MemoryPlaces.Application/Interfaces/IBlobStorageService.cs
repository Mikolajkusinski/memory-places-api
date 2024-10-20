namespace MemoryPlaces.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadBlobAsync(string blobName, Stream stream);
    Task<Stream> DownloadBlobAsync(string blobName);
    Task DeleteBlobAsync(string blobName);
}
