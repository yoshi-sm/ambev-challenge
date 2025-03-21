﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public class SaleDocumentFilter
{
    public string? SaleNumber { get; set; }
    public DateTime? SaleDateFrom { get; set; }
    public DateTime? SaleDateTo { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? BranchId { get; set; }
    public bool? IsCancelled { get; set; }
    public decimal? MinTotalAmount { get; set; }
    public decimal? MaxTotalAmount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SortField { get; set; }
    public string SortOrder { get; set; }
}

