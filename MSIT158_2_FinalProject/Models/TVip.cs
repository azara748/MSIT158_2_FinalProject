using System;
using System.Collections.Generic;

namespace MSIT158_2_FinalProject.Models;

public partial class TVip
{
    public int Vipid { get; set; }

    public string? Vipname { get; set; }

    public byte[]? Vipphoto { get; set; }

    public virtual ICollection<TMember> TMembers { get; set; } = new List<TMember>();
}
