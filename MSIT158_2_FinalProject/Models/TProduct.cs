﻿using System;
using System.Collections.Generic;

namespace MSIT158_2_FinalProject.Models;

public partial class TProduct
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? Stocks { get; set; }

    public int? LabelId { get; set; }

    public int? SubCategoryId { get; set; }

    public byte[]? ProductPhoto { get; set; }

    public decimal? UnitPrice { get; set; }

    public string? Description { get; set; }

    public int? ActiveId { get; set; }

    public DateOnly? LaunchTime { get; set; }

    public decimal? Cost { get; set; }

    public int? Status { get; set; }

    public virtual TActive? Active { get; set; }

    public virtual TLabel? Label { get; set; }

    public virtual TProductStatus? StatusNavigation { get; set; }

    public virtual TSubCategory? SubCategory { get; set; }

    public virtual ICollection<TCart> TCarts { get; set; } = new List<TCart>();

    public virtual ICollection<TKeyword> TKeywords { get; set; } = new List<TKeyword>();

    public virtual ICollection<TMemberLike> TMemberLikes { get; set; } = new List<TMemberLike>();

    public virtual ICollection<TPurchase> TPurchases { get; set; } = new List<TPurchase>();

    public virtual ICollection<TReview> TReviews { get; set; } = new List<TReview>();
}
