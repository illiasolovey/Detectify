namespace Detectify.Models;

/// <summary>
/// Represents collection of properties used for proper commnication with S3 cloud storage.
/// </summary>
public class S3Object
{
    /// <summary>
    /// Represents Object Key of S3 object.
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Represents <see cref="MemoryStream"/> object that contains the data stream to be uploaded to or downloaded from S3.
    /// </summary>
    public MemoryStream InputStream { get; set; } = null!;
}