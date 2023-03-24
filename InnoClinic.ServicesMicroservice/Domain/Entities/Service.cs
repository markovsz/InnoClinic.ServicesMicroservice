namespace Domain.Entities;

public class Service : BaseEntity
{
    public Service()
    {
    }

    public Service(Service service)
    {
        Id = service.Id;
        Name = service.Name;
        Price = service.Price;
        CategoryId = service.CategoryId;
        Category = service.Category;
        SpecializationId = service.SpecializationId;
        Specialization = service.Specialization;
        Status = service.Status;
    }

    public string Name { get; set; }
    public Decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public ServiceCategory Category { get; set; }
    public Guid SpecializationId { get; set; }
    public Specialization Specialization { get; set; }
    public string Status { get; set; }
}
