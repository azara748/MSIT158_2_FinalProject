using System;
using System.Collections.Generic;

namespace MSIT158_2_FinalProject.Models;

public partial class TStatus
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();
}
