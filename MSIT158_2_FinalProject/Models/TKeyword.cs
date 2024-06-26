﻿using System;
using System.Collections.Generic;

namespace MSIT158_2_FinalProject.Models;

public partial class TKeyword
{
    public int? ProductId { get; set; }

    public int TagId { get; set; }

    public string? Festival { get; set; }

    public string? Topic { get; set; }

    public string? Color { get; set; }

    public string? Tag { get; set; }

    public virtual TProduct? Product { get; set; }
}
