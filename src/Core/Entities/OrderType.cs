using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;
public class OrderType: BaseEntity
{

   [Required]
   public string Name { get; set; }
   public string? Abbreviation { get; set; } = null;
   
   [Required]
   public RecordStatus Status { get; set; }

   //Navigation properties
   public virtual ICollection<RestaurantOrder> RestaurantOrders { get; set; } = new List<RestaurantOrder>();

}