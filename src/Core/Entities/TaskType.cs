using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;
public class TaskType :BaseEntity
{
    [Required]
    public string Name {get; set;}=null!;

    [Required]
    public string Title {get; set;}=null!;

    //Navigation properties
    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}