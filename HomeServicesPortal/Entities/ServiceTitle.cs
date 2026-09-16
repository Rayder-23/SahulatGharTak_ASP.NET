namespace HomeServicesPortal.Entities;

public class ServiceTitle
{
    public int Uid { get; set; }

    public int CategoryUid { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    /// <summary>Admin-set price estimate for this specific title, shown read-only to customers on the request form and used to populate CustomerServiceRequests.EstimatedBudget server-side.</summary>
    public decimal? BasePrice { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedOn { get; set; }

    public ServiceCategory Category { get; set; } = null!;
}
