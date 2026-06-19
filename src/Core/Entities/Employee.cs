using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;
public class Employee: BaseEntity
{
    [Required]
    public string FirstName {get; set;} = null!;
    [Required]
    public string LastName {get; set;} = null!;
    [Required]
    public int DepartmentId {get; set;}

    //Navigation properties
    public virtual Department Department {get; set;} = null!;
    public virtual ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
}