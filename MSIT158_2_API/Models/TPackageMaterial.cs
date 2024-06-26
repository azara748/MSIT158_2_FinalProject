﻿using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TPackageMaterial
{
    public int MaterialId { get; set; }

    public string MaterialName { get; set; } = null!;

    public string? Picture { get; set; }

    public int? ColorId { get; set; }

    public string? Description { get; set; }

    public virtual TMaterialColor? Color { get; set; }

    public virtual ICollection<TAllPackage> TAllPackages { get; set; } = new List<TAllPackage>();
}
