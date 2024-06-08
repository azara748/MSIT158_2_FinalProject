using System;
using System.Collections.Generic;

namespace MSIT158_2_FinalProject.Models;

public partial class TCart
{
    public int CartId { get; set; }

    public int? MemberId { get; set; }

    public int? ProductId { get; set; }

    public int? Qty { get; set; }

    public int? PackageId { get; set; }

    public virtual TMember? Member { get; set; }

    public virtual TAllPackage? Package { get; set; }

    public virtual TProduct? Product { get; set; }
}
