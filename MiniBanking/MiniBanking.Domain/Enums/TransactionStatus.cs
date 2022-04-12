namespace MiniBanking.Domain.Enums;

public enum TransactionStatus
{
    Failed,
    Success,
    Pending,
    Created
}

public enum TransactionType
{
    TopUp,
    Transfer,
}