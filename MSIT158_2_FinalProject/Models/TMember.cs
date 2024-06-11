﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MSIT158_2_FinalProject.Models;

public partial class TMember
{
    [DisplayName("序號")]
    public int MemberId { get; set; }

    [DisplayName("姓名")]
    public string MemberName { get; set; }
    [DisplayName("手機")]
    public string Cellphone { get; set; }
    [DisplayName("地址")]
    public string Address { get; set; }
    [DisplayName("生日")]
    public DateOnly? Birthday { get; set; }
    [DisplayName("性別")]
    public string Sex { get; set; }
    [DisplayName("密碼")]
    public string Password { get; set; }
    [DisplayName("密碼加鹽")]
    public string Salt { get; set; }
    [DisplayName("電子信箱")]
    public string EMail { get; set; }
    [DisplayName("點數")]
    public int? Points { get; set; }
    [DisplayName("會員等級")]
    public int? Vipid { get; set; }
    [DisplayName("會員照片")]
    public string MemberPhoto { get; set; }
    [DisplayName("個人錢包")]
    public decimal? Wallet { get; set; }

    public virtual ICollection<TCart> TCarts { get; set; } = new List<TCart>();

    public virtual ICollection<TCollect> TCollects { get; set; } = new List<TCollect>();

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();

    public virtual ICollection<TReview> TReviews { get; set; } = new List<TReview>();

    public virtual TVip Vip { get; set; }
}