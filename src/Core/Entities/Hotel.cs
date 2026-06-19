namespace HotelManagementService.Core.Entities;

public class Hotel: BaseEntity
{
    
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int StarRating { get; set; } // 1 to 5
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

}