using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSIT158_2_API.Models;

public partial class TEmployee
{
    [Display(Name = "序號")]
    public int EmployeeId { get; set; }

    [Display(Name = "員工姓名")]
    public string? EmployeeName { get; set; }

    [DisplayName("手機")]
    public string? Cellphone { get; set; }

    [DisplayName("地址")]
    public string? Address { get; set; }

    [DisplayName("生日")]
    public string? Birthday { get; set; }

    [DisplayName("密碼")]
    public string? Password { get; set; }

    [DisplayName("電子信箱")]
    public string? EMail { get; set; }

    [DisplayName("員工照片")]
    public byte[]? EmployeePhoto { get; set; }

    [DisplayName("上班日")]
    public string? OnBoardDate { get; set; }

    [DisplayName("主管序號")]
    public int? DepId { get; set; }

    public virtual TDepartment? Dep { get; set; }

    public virtual ICollection<TEventLog> TEventLogs { get; set; } = new List<TEventLog>();
}
