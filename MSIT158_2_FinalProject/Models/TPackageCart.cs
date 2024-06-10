using System;
using System.Collections.Generic;

namespace MSIT158_2_FinalProject.Models;

public partial class TPackageCart
{
    public int PackageCartId { get; set; }

    public int? MemberId { get; set; }

    public int? PackageId { get; set; }

    public int? Qty { get; set; }

    public virtual TMember? Member { get; set; }

    public virtual TAllPackage? Package { get; set; }
}
