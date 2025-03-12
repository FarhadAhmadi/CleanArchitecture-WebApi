using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Models.Base;

namespace CleanArchitecture.Shared.Models.Book.Requests;
public class BookSearchRequest : BaseSearchRequest
{
    public string? AuthorId { get; set; }
    public string? PublisherId { get; set; }
    public bool? availability { get; set; }
}
