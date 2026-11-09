using System.Text.RegularExpressions;
using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Services;

public static class PasswordPolicy
{
    public static List<string> Validate(string password, SecuritySettingsDto settings)
    {
        var errors = new List<string>();
        if (password.Length < settings.PasswordMinLength)
            errors.Add($"At least {settings.PasswordMinLength} characters");

        if (settings.RequireStrongPassword)
        {
            if (!Regex.IsMatch(password, "[A-Z]")) errors.Add("One uppercase letter");
            if (!Regex.IsMatch(password, "[a-z]")) errors.Add("One lowercase letter");
            if (!Regex.IsMatch(password, "[0-9]")) errors.Add("One number");
            if (!Regex.IsMatch(password, "[^A-Za-z0-9]")) errors.Add("One special character");
        }
        return errors;
    }
}