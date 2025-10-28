using System;
using System.Collections.Generic;

namespace TaskManagment.Models;

public partial class UserToken
{
    public int TokenId { get; set; }

    public int? UserId { get; set; }

    public string Token { get; set; } = null!;

    public string? TokenType { get; set; }

    public DateTime ExpiryTime { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
