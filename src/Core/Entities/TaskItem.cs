using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;
public class TaskItem : BaseEntity
{
    [Required]
    public int HotelId { get; set; }

    public int RoomId {get; set;}

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = null!;

    [Required]
    public int TaskTypeId { get; set; } 

    public int? AssignedToId { get; set; }

    [Required]
    public Priorities Priority {get; set;}

    public DateTime? StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    [Required]
    public RecordStatus Status { get; set; } 

    [MaxLength(1000)]
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Hotel Hotel { get; set; } = null!;
    public virtual Employee? AssignedTo { get; set; }
    public virtual TaskType TaskType {get; set;} = null!;
    public virtual Room Room { get; set; } = null!;
}