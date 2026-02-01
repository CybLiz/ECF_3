using System.Net.Http.Json;
using BookHub.Shared.DTOs;

namespace BookHub.BlazorClient.Services;

public class LoanService : ILoanService
{
    private readonly HttpClient _httpClient;

    public LoanService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoanDto> CreateLoanAsync(CreateLoanDto createLoanDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/loans", createLoanDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LoanDto>()!;
    }

    public async Task<IEnumerable<LoanDto>> GetUserLoansAsync(Guid userId)
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<LoanDto>>($"api/loans/user/{userId}");
        return response ?? Enumerable.Empty<LoanDto>();
    }

    public async Task<IEnumerable<LoanDto>> GetOverdueLoansAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<LoanDto>>("api/loans/overdue");
        return response ?? Enumerable.Empty<LoanDto>();
    }

    public async Task<LoanDto?> ReturnLoanAsync(Guid loanId)
    {
        var response = await _httpClient.PutAsync($"api/loans/{loanId}/return", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LoanDto>();
    }
}
