using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSIT158_2_FinalProject.Models;

public partial class TProduct
{
    [Display(Name = "ID")]
    public int ProductId { get; set; }

    [Display(Name = "名稱")]
    public string? ProductName { get; set; }

    [Display(Name = "庫存")]
    public int? Stocks { get; set; }

    [Display(Name = "品牌")]
    public int? LabelId { get; set; }

    [Display(Name = "分類")]
    public int? SubCategoryId { get; set; }

    [Display(Name = "照片")]
    public byte[]? ProductPhoto { get; set; }

    [Display(Name = "售價")]
    public decimal? UnitPrice { get; set; }

    [Display(Name = "說明")]
    public string? Description { get; set; }

    [Display(Name = "活動")]
    public int? ActiveId { get; set; }

    [Display(Name = "日期")]
    public DateTime? LaunchTime { get; set; }

    [Display(Name = "成本價")]
    public decimal? Cost { get; set; }

    public virtual TActive? Active { get; set; }

    public virtual TLabel? Label { get; set; }

    public virtual TSubCategory? SubCategory { get; set; }

    public virtual ICollection<TCart> TCarts { get; set; } = new List<TCart>();

    public virtual ICollection<TCollect> TCollects { get; set; } = new List<TCollect>();

    public virtual ICollection<TKeyword> TKeywords { get; set; } = new List<TKeyword>();

    public virtual ICollection<TMemberLike> TMemberLikes { get; set; } = new List<TMemberLike>();

    public virtual ICollection<TPurchase> TPurchases { get; set; } = new List<TPurchase>();

    public virtual ICollection<TReview> TReviews { get; set; } = new List<TReview>();
}
