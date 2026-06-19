using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace HotelManagementService.Core.Entities;
public class GuestPreferences: BaseEntity
{
    [Required]
    public int GuestId {get; set;}
    
    [Required]
    [ForeignKey(nameof(Preferences))]
    public int PreferenceId {get; set;}

    
    // 🔗 Navigation Properties
    public virtual Guest Guest { get; set; } = null!;
    public virtual Preferences Preferences { get; set; } = null!;
}