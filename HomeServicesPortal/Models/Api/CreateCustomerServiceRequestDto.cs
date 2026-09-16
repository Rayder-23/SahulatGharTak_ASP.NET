using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HomeServicesPortal.Models.Api;

public class CreateCustomerServiceRequestDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ClientUid { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int CategoryUid { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ClientAddressUid { get; set; }

    [Required(ErrorMessage = "Service title is required.")]
    [StringLength(150)]
    public string ServiceTitle { get; set; } = string.Empty;

    /// <summary>
    /// Optional. ServiceTitles.UID for the selected title (see GET /api/service-titles?categoryUid={id}).
    /// When provided, the request's estimated budget is populated server-side from
    /// ServiceTitles.BasePrice — the title must exist, be active, and belong to CategoryUid, else 400.
    /// The client can no longer set EstimatedBudget directly; omit this field to leave it unset.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int? ServiceTitleUid { get; set; }

    /// <summary>Optional. Omit, null, or empty string when no description is provided.</summary>
    [StringLength(4000)]
    public string? ServiceDescription { get; set; }

    /// <summary>Optional. Omit, null, or empty string when the client has no preferred date.</summary>
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? PreferredServiceDate { get; set; }

    /// <summary>Optional. Omit, null, or empty string when the client has no preferred time.</summary>
    [StringLength(50)]
    public string? PreferredServiceTime { get; set; }

    public bool IsUrgent { get; set; }

    [StringLength(150)]
    public string? ContactPerson { get; set; }

    [Required(ErrorMessage = "Contact number is required.")]
    [StringLength(20)]
    public string ContactNo { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Remarks { get; set; }
}
