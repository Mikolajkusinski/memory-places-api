namespace MemoryPlaces.Application.Extensions;

public static class StreamExtensions
{
    public static byte[] ToByteArray(this Stream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
