using System;
using System.Collections.Generic;

namespace MSIT158_2_FinalProject.Models;

public partial class TLabel
{
    public int LabelId { get; set; }

    /// <summary>
    /// 原為供應商名稱、現改為品牌；為維持原有 Database Model 架構，表格名稱不更動
    /// </summary>
    public string? LabelName { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public byte[]? SupplierPhoto { get; set; }

    public virtual ICollection<TProduct> TProducts { get; set; } = new List<TProduct>();
}
