using Azure.Storage.Blobs;
using MemoryPlaces.Application.Interfaces;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadBlobAsync(string blobName, Stream stream)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient("images");
        var blobClient = blobContainerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(stream);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadBlobAsync(string blobName)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient("images");
        var blobClient = blobContainerClient.GetBlobClient(blobName);
        var downloadInfo = await blobClient.DownloadAsync();
        return downloadInfo.Value.Content;
    }

    public async Task DeleteBlobAsync(string blobName)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient("images");
        var blobClient = blobContainerClient.GetBlobClient(blobName);
        await blobClient.DeleteAsync();
    }
}
