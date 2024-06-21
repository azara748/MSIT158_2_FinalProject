using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TActiveImage
{
    public int ImageId { get; set; }

    public byte[] ActiveImage { get; set; } = null!;
}
