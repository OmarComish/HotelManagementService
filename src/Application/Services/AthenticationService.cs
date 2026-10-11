using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using Microsoft.Extensions.Configuration;   

namespace HotelManagementService.Application.Services;
public class AuthenticationService : IAuthenticationService
{
    private readonly IApiService _apiservice;
    private readonly IConfiguration _config;
    public AuthenticationService(IApiService apiservice, IConfiguration config)
    {
        _apiservice = apiservice;
        _config = config;
    }
    public async Task<AuthResponseDto> AuthenticateUser(LogInRequestDto dto)
    {
        //var response = new AuthResponseDto {Status ="error", Message ="Invalid username or password"};
        Console.WriteLine("Requesting signin to identity server...");

        var userdetails = await _apiservice.SignInAsync(dto);

        if(userdetails == null)
        {
            return new AuthResponseDto
            {
                Status ="error",
                Message ="Invalid username or password"
            };
        } 

        Console.WriteLine("Signin successful...");
        var tokenRequest = new TokenRequestDto
        {
            GrantType = _config["IdentityService:grant_type"],
            Username = dto.Username,
            Password = dto.Password,
            ClientId = _config["IdentityService:client_id"],
            ClientSecret = _config["IdentityService:client_secret"],
            Scope = _config["IdentityService:scope"],
        };
     
        Console.WriteLine($"Logging tokenRequest Object...");
        Console.WriteLine($"{_config["IdentityService:grant_type"]}");
        Console.WriteLine($"{_config["IdentityService:client_id"]}");
        Console.WriteLine($"{_config["IdentityService:client_secret"]}");
        Console.WriteLine($"{_config["IdentityService:scope"]}");
        Console.WriteLine("Requesting token from identity server...");

        var tokenResponse = await _apiservice.GetTokenAsync(tokenRequest);

        Console.WriteLine();
        Console.WriteLine("=== RESULT ===");

        if(tokenResponse == null)
        {
            return new AuthResponseDto
            {
                Status = "error",
                Message = "Token endpoint returned null"
            };
        }

        if(!string.IsNullOrEmpty(tokenResponse.AccessToken))
        {
            return new AuthResponseDto
            {
                Status ="success",
                Message =$"Successfully authenticated profile for {dto.Username}",
                Token = tokenResponse.AccessToken
            };
        }

        return new AuthResponseDto
        {
            Status ="error",
            Message = tokenResponse.ErrorDescription?? "Token request failed"
        };

    }

    public async Task<ResponseDto> RegisterUser(RegisterUserDto dto)
    {
        var response = new ResponseDto {Message = "An error occurred. We failed to register user", Status ="error"};
        if(dto == null) 
           return response;
        response = await _apiservice.RegisterUserAsync(dto);
        return response;
    }
}