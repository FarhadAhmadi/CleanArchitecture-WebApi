using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Shared.Models.Book;
public class AuthorDTO : BaseDTO
{
    public string Name { get; set; }
    public string Bio { get; set; }
}
