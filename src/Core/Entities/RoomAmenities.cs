using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;
public class RoomAmenities: BaseEntity
{
    public int RoomTypeId { get; set; }
    
    //Navigation properties
    public int AmenitiesId {get; set;}
    public Amenities Amenities { get; set; } = null!;
    public virtual ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();
}