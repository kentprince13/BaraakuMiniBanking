using MiniBanking.Core.Models;

namespace MiniBanking.API.Models;

public class StandardErrorResponse:BaseModel
{
    public bool Status { get; set; }
    public string Code { get; set; }
    public string StatusMessage { get; set; }
}