namespace Examples.Service.Domain.Interfaces
{
    public interface IEntity
    {
        Guid? Id { get; set; }
        DateTime? CreateDate { get; set; }
        DateTime? UpdateDate { get; set; }
    }
}
