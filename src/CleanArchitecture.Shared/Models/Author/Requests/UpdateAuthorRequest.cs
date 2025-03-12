using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Shared.Models.Author.Requests;
public class UpdateAuthorRequest
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Bio { get; set; }

}
