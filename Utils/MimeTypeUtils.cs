namespace Detectify.Utils;

public static class MimeTypeUtils
{
    public static string GetContentType(string filename)
    {
        string extension = Path.GetExtension(filename);
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            _ => "application/octet-stream"
        };
    }
}