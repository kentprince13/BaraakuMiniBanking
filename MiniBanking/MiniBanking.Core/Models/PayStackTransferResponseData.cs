namespace MiniBanking.Core.Models;

public class PayStackTransferResponseData:BaseModel
{
    public string Domain { get; set; }
    public int Amount { get; set; }
    public string Currency { get; set; }
    public string Reference { get; set; }
    public string Source { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
    public object Failures { get; set; }
    public string TransferCode { get; set; }
    public int Id { get; set; }
    public int Integration { get; set; }
    public int Request { get; set; }
    public int Recipient { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
public class PayStackTransferResponse:BaseModel
{
    public bool Status { get; set; }
    public string Message { get; set; }
    public PayStackTransferResponseData Data { get; set; }
}