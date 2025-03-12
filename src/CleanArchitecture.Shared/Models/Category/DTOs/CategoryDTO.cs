using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Models.Base;

namespace CleanArchitecture.Shared.Models.Category.DTOs;
public class CategoryDTO : BaseDTO
{
    public string Name { get; set; }
}
