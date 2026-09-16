namespace HomeServicesPortal.Models.Api;

public class ServiceTitleApiDto
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    /// <summary>Admin-set price estimate for this title (ServiceTitles.BasePrice). Null if not set. Show as the read-only estimated budget on the service-request form.</summary>
    public decimal? BasePrice { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime? CreatedOn { get; set; }
}
