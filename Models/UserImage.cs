using System;
using System.Collections.Generic;

namespace TaskManagment.Models;

public partial class UserImage
{
    public int UserImageId { get; set; }

    public byte[]? ImageData { get; set; }

    public string? ContentType { get; set; }

    public DateTime? UploadDate { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
