using MiniBanking.Core.Models;

namespace MiniBanking.Core.Services;

public interface ITransferService
{
    Task<PayStackResponseModel> Transfer(PayStackRequestModel request);
    Task<PayStackResponseModel> TopUp(TopUpRequest request);
}