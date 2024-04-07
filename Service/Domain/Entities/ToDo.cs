namespace Examples.Service.Domain.Entities
{
    public class ToDo : IEntity
    {
        public Guid? Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; } = string.Empty;
        public bool? Done { get; set; } = false;
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}
