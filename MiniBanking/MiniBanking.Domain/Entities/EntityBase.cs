namespace MiniBanking.Domain.Entities;

public class EntityBase
{
    public long Id { get; protected set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}