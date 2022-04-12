using MiniBanking.Core.Models;

namespace MiniBanking.API.Models;

public class TransferResponse:BaseModel
{
    public string Status { get; set; }
    public string Message { get; set; }
}