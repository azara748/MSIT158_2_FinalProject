using System;
using System.Collections.Generic;

namespace MSIT158_2_FinalProject.Models;

public partial class TMemberLike
{
    public int LikeId { get; set; }

    public int? MemeberId { get; set; }

    public int? ProductId { get; set; }

    public virtual TMember? Memeber { get; set; }

    public virtual TProduct? Product { get; set; }
}
