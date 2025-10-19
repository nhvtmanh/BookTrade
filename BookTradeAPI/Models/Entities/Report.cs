using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class Report
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public byte Type { get; set; }

    public int TargetId { get; set; }

    public int? ReporterId { get; set; }

    public virtual User? Reporter { get; set; }
}
