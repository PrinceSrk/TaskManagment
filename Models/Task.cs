using System;
using System.Collections.Generic;

namespace TaskManagment.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? AssignedTo { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly DueDate { get; set; }

    public virtual User? AssignedToNavigation { get; set; }
}
