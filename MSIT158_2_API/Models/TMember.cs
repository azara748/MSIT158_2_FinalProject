using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TMember
{
    public int MemberId { get; set; }

    public string? MemberName { get; set; }

    public string? Cellphone { get; set; }

    public string? Address { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Sex { get; set; }

    public string? Password { get; set; }

    public string? Salt { get; set; }

    public string? EMail { get; set; }

    public int? Points { get; set; }

    public int? Vipid { get; set; }

    public string? MemberPhoto { get; set; }

    public decimal? Wallet { get; set; }

    public virtual ICollection<TCart> TCarts { get; set; } = new List<TCart>();

    public virtual ICollection<TCollect> TCollects { get; set; } = new List<TCollect>();

    public virtual ICollection<TMemberLike> TMemberLikes { get; set; } = new List<TMemberLike>();

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();

    public virtual ICollection<TPackageCart> TPackageCarts { get; set; } = new List<TPackageCart>();

    public virtual ICollection<TReview> TReviews { get; set; } = new List<TReview>();

    public virtual TVip? Vip { get; set; }
}
