using System;
using System.Collections.Generic;

namespace TaskManagment.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<UserImage> UserImages { get; set; } = new List<UserImage>();

    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
}
