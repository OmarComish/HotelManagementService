namespace HotelManagementService.Core.Entities;
public class InvoiceLineItem: BaseEntity
{
    public int InvoiceId {get; set;}
    public string? Description { get; set; } //Room 301 -- 3 nights
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;

    //Nav properties
    public virtual Invoice Invoice {get; set;} = null!;
}