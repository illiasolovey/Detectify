namespace Detectify.Utils;

public static class StorageServiceUtils
{
    public static string GenerateUUIDFilename(string filename)
    {
        string extension = Path.GetExtension(filename);
        var uuid = Guid.NewGuid().ToString("D");
        return $"Detectify:{uuid}{extension}";
    }
}