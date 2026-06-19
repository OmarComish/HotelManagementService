using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;
public class Department: BaseEntity
{
    [Required]
    public string Name {get; set;} = null!;

    //navigation properties
    public virtual ICollection<Employee> Employees {get; set;} = new List<Employee>();
}