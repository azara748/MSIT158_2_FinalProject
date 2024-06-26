﻿using System;
using System.Collections.Generic;

namespace MSIT158_2_API.Models;

public partial class TSubCategory
{
    public int SubCategoryId { get; set; }

    public string? SubCategoryName { get; set; }

    public string? SubCategoryCname { get; set; }

    public int? CategoryId { get; set; }

    public virtual TCategory? Category { get; set; }

    public virtual ICollection<TProduct> TProducts { get; set; } = new List<TProduct>();
}
