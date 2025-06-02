namespace Core.DTOs
{
    public class CsvUploadRequest
    {
        public Stream FileStream { get; set; } = Stream.Null;
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
    }
} 