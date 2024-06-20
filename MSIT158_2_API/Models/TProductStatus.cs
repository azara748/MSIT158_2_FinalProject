using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TProductStatus
{
    public int ProductStatusId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<TProduct> TProducts { get; set; } = new List<TProduct>();
}
