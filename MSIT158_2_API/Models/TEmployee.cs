using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TEmployee
{
    public int EmployeeId { get; set; }

    public string? EmployeeName { get; set; }

    public string? Cellphone { get; set; }

    public string? Address { get; set; }

    public string? Birthday { get; set; }

    public string? Password { get; set; }

    public string? EMail { get; set; }

    public byte[]? EmployeePhoto { get; set; }

    public string? OnBoardDate { get; set; }

    public int? DepId { get; set; }

    public virtual TDepartment? Dep { get; set; }

    public virtual ICollection<TEventLog> TEventLogs { get; set; } = new List<TEventLog>();
}
