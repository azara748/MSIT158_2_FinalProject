﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TPurchase
{
    public int OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Qty { get; set; }

    public string Memo { get; set; }

    public int PurchaseId { get; set; }

    public decimal? PurchaseAmount { get; set; }

    public virtual TOrder Order { get; set; }

    public virtual TProduct Product { get; set; }
}