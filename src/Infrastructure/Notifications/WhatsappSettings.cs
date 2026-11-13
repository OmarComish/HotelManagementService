namespace HotelManagementService.Infrastructure.Notifications;

public class WhatsAppSettings
{
    public bool Enabled { get; set; } = false;
    public string ApiVersion { get; set; } = string.Empty;
    public string PhoneNumberId { get; set; } = string.Empty;
    public string BusinessAccountId { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string TemplateName { get; set; } = "reservation_confirmation";
    public string LanguageCode { get; set; } = "en";
   
}