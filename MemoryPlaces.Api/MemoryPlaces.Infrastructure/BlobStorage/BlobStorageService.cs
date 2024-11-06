using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MemoryPlaces.Application.Image;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Domain.Entities;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadBlobAsync(ImageDto image)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("images");
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(image.FileName);

        using (var stream = new MemoryStream(image.Content))
        {
            await blobClient.UploadAsync(
                stream,
                new BlobHttpHeaders { ContentType = image.ContentType }
            );
        }
        return blobClient.Uri.ToString();
    }

    public async Task DeleteBlobAsync(string blobName)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient("images");
        var blobClient = blobContainerClient.GetBlobClient(blobName);
        await blobClient.DeleteAsync();
    }
}
