using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChoice.WebApis.Domain.Entities;

[Table("LocationService")]
public class LocServiceEntity
{
    [Key]
    [Column("LocationServiceID")]
    public string LocationServiceId { get; set; } = default!;

    [Column("LocId")]
    public string? LocId { get; set; }

    [Column("ExternalLocId")]
    public string? ExternalLocId { get; set; }

    [Column("ServiceID")]
    public string? ServiceId { get; set; }
    public string? ServiceID { get;  set; }
    [Column("ProviderName")]
    public string? ProviderName { get; set; }

    [Column("PartnerCode")]
    public string? PartnerCode { get; set; }

    [Column("ServiceType")]
    public string? ServiceType { get; set; }

    [Column("ServiceName")]
    public string? ServiceName { get; set; }

    [Column("ServiceCode")]
    public string? ServiceCode { get; set; }

    // Kept as string to preserve Java behavior & DB parity
    [Column("MinAmount")]
    public string? MinAmount { get; set; }

    // Kept as string to preserve Java behavior & DB parity
    [Column("MaxAmount")]
    public string? MaxAmount { get; set; }

    [Column("CurrencyCode")]
    public string? CurrencyCode { get; set; }

    [Column("CountryCode")]
    public string? CountryCode { get; set; }

    [Column("Description")]
    public string? Description { get; set; }

    [Column("Status")]
    public bool Active { get; set; }
}
