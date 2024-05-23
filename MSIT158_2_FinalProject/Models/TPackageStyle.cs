using System;
using System.Collections.Generic;

namespace MSIT158_2_FinalProject.Models;

public partial class TPackageStyle
{
    public int PackageStyleId { get; set; }

    public string StyleName { get; set; } = null!;

    public string? Picture { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TAllPackage> TAllPackages { get; set; } = new List<TAllPackage>();
}
