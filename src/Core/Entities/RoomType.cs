

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementService.Core.Entities;

public class RoomType:BaseEntity
{
   
    [Required]
    public string Type { get; set; } = null!; //Standard,Delux, Suite
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    [Required]
    public int Capacity { get; set; }

    //Navigation properties
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    public virtual ICollection<RoomAmenities> Amenities {get; set;} = new List<RoomAmenities>();
    
}