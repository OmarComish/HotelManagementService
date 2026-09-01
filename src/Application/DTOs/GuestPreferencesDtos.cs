using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Application.DTOs;
public record CreateGuestPreferenceDto(
    [Required] int GuestId,
    [Required] int PreferenceId
);

public record UpdateGuestPreferenceDto(
    [Required] int Id,
    [Required] int GuestId,
    [Required] int PreferenceId
); 

public record GuestPreferenceDto
{
    public int Id {get; set;}
    public string? PreferenceName {get; set;} = null;
    public string? CreatedBy { get; set; }=null;
    public DateTime DateCreated { get; set; }
    public string? Status { get; set; }
}

public record PreferenceDto
{
    public int Id {get; set;}
    public string? Name {get; set;} = null;
    public string? CreatedBy { get; set; }=null;
    public DateTime DateCreated { get; set; }
    public string? Status { get; set; }
}
public record CreatePreferenceDto(
    [Required] string Name
);
public record UpdatePreferenceDto(
    [Required] int Id,
    [Required] string Name
);