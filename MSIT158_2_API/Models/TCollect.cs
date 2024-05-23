using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TCollect
{
    public int CollectId { get; set; }

    public int? MemberId { get; set; }

    public int? ProductId { get; set; }

    public virtual TMember? Member { get; set; }

    public virtual TProduct? Product { get; set; }
}
