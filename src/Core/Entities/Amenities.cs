using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementService.Core.Entities;

public class Amenities : BaseEntity
{
    [Required]
    public string Name { get; set; } = null!;

    //Navigation properties
    public ICollection<RoomAmenities> RoomAmenities {get; set;} = new List<RoomAmenities>();
}