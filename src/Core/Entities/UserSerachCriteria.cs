namespace HotelManagementService.Core.Entities;
public class UserSearchCriteria
{
    public string? Search { get; set; }
    public string? RoleKey { get; set; }
    public RecordStatus? Status { get; set; }
    public DateTime? CreatedFromUtc { get; set; }
    public DateTime? CreatedToExclusiveUtc { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; } = 10;
}