﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT158_2_FinalProject.Models;

public partial class TCollect
{
    public int CollectId { get; set; }

    public int? MemberId { get; set; }

    public int? ProductId { get; set; }

    public virtual TMember Member { get; set; }

    public virtual TProduct Product { get; set; }
}