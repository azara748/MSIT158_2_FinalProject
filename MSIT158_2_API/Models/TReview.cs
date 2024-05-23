using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TReview
{
    public int ReviewId { get; set; }

    public int? MemberId { get; set; }

    public int? ProductId { get; set; }

    public int? RankId { get; set; }

    public string? Comment { get; set; }

    public string? ReviewDate { get; set; }

    public int? PurchaseId { get; set; }

    public virtual TMember? Member { get; set; }

    public virtual TProduct? Product { get; set; }

    public virtual TAppraisal? Rank { get; set; }
}
