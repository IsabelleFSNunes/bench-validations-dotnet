using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimal_api_net7_dataannotations.Features.Posts.DTOs
{
    public record PostResponse(int Id, string Title, string Body, int UserId, int Views, List<string> Tags);
}
