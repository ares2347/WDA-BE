namespace WDA.Service.Attachment;

public interface IAttachmentService
{
    Task<string> SaveFileAsync(Stream file, string fileName);
    FileStream BrowseFile(string path);
    Task<MemoryStream> GetFileMemoryStream(string path);
    void RemoveFile(string path);
}