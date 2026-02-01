using BookHub.Shared.DTOs;

namespace BookHub.LoanService.Domain.Ports;

public interface IUserServiceClient
{
/*    Task<UserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
*/    Task<UserDto?> GetUserAsync(Guid userId, string token, CancellationToken cancellationToken = default);

}
