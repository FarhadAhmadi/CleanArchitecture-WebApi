using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Shared.Models.Base;
public class BaseSearchRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be at least 1.")]
    public int PageIndex { get; set; } = 1;

    [Range(10, 200, ErrorMessage = "PageSize must be at least 10.")]
    public int PageSize { get; set; } = 10;

    //public string? orderBy { get; set; }  // Column to sort by (e.g., "Name")
    public bool IsDescending { get; set; } = false; // Sort direction
}

