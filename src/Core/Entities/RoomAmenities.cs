using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementService.Core.Entities;
public class RoomAmenities: BaseEntity
{
    public int RoomTypeId { get; set; }
    
    //Navigation properties
    public int AmenitiesId {get; set;}
    //public Amenities Amenities { get; set; } = null!;

    //Navigation property to RoomType
    [ForeignKey(nameof(RoomTypeId))]
    public virtual RoomType RoomType { get; set; } = null!;

    //Navigation property to Amenities
    [ForeignKey(nameof(AmenitiesId))]   
    public virtual Amenities Amenities { get; set; } = null!;

    //Navigation property to RoomTypes
    //public virtual ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();
}