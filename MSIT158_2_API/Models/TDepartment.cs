using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TDepartment
{
    public int DepId { get; set; }

    public string? DepName { get; set; }

    public virtual ICollection<TEmployee> TEmployees { get; set; } = new List<TEmployee>();
}
