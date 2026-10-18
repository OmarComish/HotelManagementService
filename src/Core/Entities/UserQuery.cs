namespace HotelManagementService.Core.Entities;
public class UserQuery
{
    public string? Search { get; set; }
    public string? Role { get; set; }       // role key, e.g. "manager"; null or "all" = any
    public string? Status { get; set; }     // "active" | "inactive"; null or "all" = any
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public string? SortBy { get; set; }     // name (default) | username | email | status | createdAt
    public string? SortDir { get; set; }    // asc (default) | desc
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}