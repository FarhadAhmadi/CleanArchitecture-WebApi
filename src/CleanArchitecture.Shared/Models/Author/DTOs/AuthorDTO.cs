using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Models.Base;

namespace CleanArchitecture.Shared.Models.Author.DTOs;
public class AuthorDTO : BaseDTO
{
    public string Name { get; set; }
    public string Bio { get; set; }
}
