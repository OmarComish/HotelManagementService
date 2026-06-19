using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementService.Core.Entities;
public class Payment : BaseEntity
{
    [Required]
    public int InvoiceId { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(20)]
    public string? Reference { get; set; } //Txn reference id

    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = null!;

    public DateTime? ProcessedAt { get; set; }

    public string? Status {get; set;}

    // Navigation properties
    //public virtual Booking Booking { get; set; } = null!;
}