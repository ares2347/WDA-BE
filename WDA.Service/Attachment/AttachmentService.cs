using System.Net;
using Azure.Storage.Blobs;
using WDA.Shared;

namespace WDA.Service.Attachment;

public class AttachmentService : IAttachmentService
{
    public async Task<string> SaveFileAsync(Stream file, string fileName)
    {
        var container = new BlobContainerClient(AppSettings.Instance.AzureStorage.ConnectionString,
            "attachments");
        try
        {
            var blob = container.GetBlobClient(fileName);
            await blob.UploadAsync(file);
            var fileUrl = blob.Uri.AbsoluteUri;
            return fileUrl;
        }
        catch (Exception ex)
        {
            throw new HttpException("Upload file failed.", HttpStatusCode.BadRequest);
        }
    }

    public async Task<Stream> BrowseFile(string fileName)
    {
        try
        {
            var container = new BlobContainerClient(AppSettings.Instance.AzureStorage.ConnectionString,
                "attachments");
            var blob = container.GetBlobClient(fileName);
            var stream = new MemoryStream();
            await blob.DownloadToAsync(stream);
            return stream;
        }
        catch (Exception e)
        {
            throw new HttpException("Get file failed.", HttpStatusCode.BadRequest);
        }
    }

    public async Task<MemoryStream> GetFileMemoryStream(string path)
    {
        HttpException.ThrowIfNull(path);
        var stream = new MemoryStream();
        await using var fs = File.Open(path, FileMode.Open);
        await fs.CopyToAsync(stream);
        return stream;
    }

    public void RemoveFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new HttpException("Remove file failed", HttpStatusCode.BadRequest);
        }

        File.Delete(path);
    }
}