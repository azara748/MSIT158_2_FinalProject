﻿using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TWordSeverity
{
    public int WordSeverityId { get; set; }

    public string? Severitylevel { get; set; }

    public virtual ICollection<TWordCensorship> TWordCensorships { get; set; } = new List<TWordCensorship>();
}
