using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TWordCensorship
{
    public int WordCensorshipId { get; set; }

    public string? Word { get; set; }

    public int? WordSeverityId { get; set; }

    public virtual TWordSeverity? WordSeverity { get; set; }
}
