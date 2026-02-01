using System.Net.Http.Json;
using BookHub.Shared.DTOs;

namespace BookHub.BlazorClient.Services;

public class UserApiService
{
    private readonly HttpClient _httpClient;

    public UserApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDto>> GetAllAsync() =>
        await _httpClient.GetFromJsonAsync<List<UserDto>>("api/users");

    public async Task<UserDto> GetByIdAsync(Guid id) =>
        await _httpClient.GetFromJsonAsync<UserDto>($"api/users/{id}");

    public async Task UpdateAsync(Guid id, UpdateUserDto dto) =>
        await _httpClient.PutAsJsonAsync($"api/users/{id}", dto);

    public async Task DeleteAsync(Guid id) =>
        await _httpClient.DeleteAsync($"api/users/{id}");
}
