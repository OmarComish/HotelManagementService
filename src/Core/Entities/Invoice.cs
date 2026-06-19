using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementService.Core.Entities;
public class Invoice : BaseEntity
{
    [Required]
    public int ReservationId { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Tax { get; set; }

    [Required]
    public DateTime IssuedDate { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public string? InvoiceNumber { get; set; }     // e.g. "INV-2026-00042"

    [Required]
    [MaxLength(20)]
    public InvoiceStatus Status { get; set; }

    public string? PaymentMethod {get; set;}

    // Navigation properties
    //public virtual Reservation Reservation { get; set; } = null!;
    public virtual ICollection<InvoiceLineItem> LineItems { get; set; }= new List<InvoiceLineItem>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}