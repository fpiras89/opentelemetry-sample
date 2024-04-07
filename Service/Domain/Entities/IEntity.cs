namespace Examples.Service.Domain.Entities
{
    public interface IEntity
    {
        Guid? Id { get; set; }
        DateTime? CreateDate { get; set; }
        DateTime? UpdateDate { get; set; }
    }
}
