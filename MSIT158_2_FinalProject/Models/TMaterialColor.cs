using System;
using System.Collections.Generic;

namespace MSIT158_2_FinalProject.Models;

public partial class TMaterialColor
{
    public int ColorId { get; set; }

    public string ColorName { get; set; } = null!;

    public string? Rgb { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TPackageMaterial> TPackageMaterials { get; set; } = new List<TPackageMaterial>();
}
