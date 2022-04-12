namespace MiniBanking.Core.Configuration;

public class PayStackSettings
{
    public string BaseUrl { get; set; }
    public string Secret { get; set; }
    public string TransferUrl { get; set; }
    public string RecipientUrl { get; set; }
}