using System.Net.Http.Json;
using BookHub.Shared.DTOs;
using Blazored.LocalStorage;


namespace BookHub.BlazorClient.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users/login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            if (result != null)
            {
                // Stocker token et user dans LocalStorage
                await _localStorage.SetItemAsync("authToken", result.Token);
                await _localStorage.SetItemAsync("currentUser", result.User);

                // Ajout du JWT au HttpClient
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);
            }

            return result;
        }

        return null;
    }

    public async Task<UserDto> RegisterAsync(CreateUserDto createUserDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users/register", createUserDto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<UserDto>())!;
    }
}
