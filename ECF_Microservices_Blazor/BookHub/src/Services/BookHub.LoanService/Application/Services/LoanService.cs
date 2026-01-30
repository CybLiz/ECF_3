using BookHub.LoanService.Domain.Entities;
using BookHub.LoanService.Domain.Ports;
using BookHub.Shared.DTOs;
using Microsoft.Extensions.Logging;

namespace BookHub.LoanService.Application.Services;

public interface ILoanService
{
    Task<IEnumerable<LoanDto>> GetAllLoansAsync(CancellationToken cancellationToken = default);
    Task<LoanDto?> GetLoanByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanDto>> GetLoansByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanDto>> GetOverdueLoansAsync(CancellationToken cancellationToken = default);
    Task<LoanDto> CreateLoanAsync(CreateLoanDto dto, CancellationToken cancellationToken = default);
    Task<LoanDto?> ReturnLoanAsync(Guid id, CancellationToken cancellationToken = default);
}

public class LoanService : ILoanService
{
    private readonly ILoanRepository _repository;
    private readonly ICatalogServiceClient _catalogClient;
    private readonly IUserServiceClient _userClient;
    private readonly ILogger<LoanService> _logger;

    public LoanService(
        ILoanRepository repository,
        ICatalogServiceClient catalogClient,
        IUserServiceClient userClient,
        ILogger<LoanService> logger)
    {
        _repository = repository;
        _catalogClient = catalogClient;
        _userClient = userClient;
        _logger = logger;
    }

    public async Task<IEnumerable<LoanDto>> GetAllLoansAsync(CancellationToken cancellationToken = default)
    {
        var loans = await _repository.GetAllAsync(cancellationToken);
        return loans.Select(MapToDto);
    }

    public async Task<LoanDto?> GetLoanByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var loan = await _repository.GetByIdAsync(id, cancellationToken);
        return loan == null ? null : MapToDto(loan);
    }

    public async Task<IEnumerable<LoanDto>> GetLoansByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var loans = await _repository.GetByUserIdAsync(userId, cancellationToken);
        return loans.Select(MapToDto);
    }

    public async Task<IEnumerable<LoanDto>> GetOverdueLoansAsync(CancellationToken cancellationToken = default)
    {
        var overdueLoans = await _repository.GetOverdueLoansAsync(cancellationToken);
        return overdueLoans.Select(MapToDto);
    }

    public async Task<LoanDto> CreateLoanAsync(CreateLoanDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _userClient.GetUserAsync(dto.UserId, cancellationToken);
        if (user == null)
            throw new InvalidOperationException("User not found");

        var book = await _catalogClient.GetBookAsync(dto.BookId, cancellationToken);
        if (book == null)
            throw new InvalidOperationException("Book not found");

        if (!book.IsAvailable)
            throw new InvalidOperationException("Book is not available");

        var activeLoans = await _repository.GetActiveLoansCountByUserAsync(dto.UserId, cancellationToken);
        if (activeLoans >= Loan.MaxActiveLoansPerUser)
            throw new InvalidOperationException("User has reached max active loans");

        var loan = Loan.Create(dto.UserId, dto.BookId, book.Title, user.Email);
        await _repository.AddAsync(loan, cancellationToken);

        var decremented = await _catalogClient.DecrementAvailabilityAsync(dto.BookId, cancellationToken);
        if (!decremented)
            _logger.LogWarning("Failed to decrement book availability for book {BookId}", dto.BookId);

        return MapToDto(loan);
    }

    public async Task<LoanDto?> ReturnLoanAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var loan = await _repository.GetByIdAsync(id, cancellationToken);
        if (loan == null) return null;
        if (loan.Status != BookHub.LoanService.Domain.Entities.LoanStatus.Active)
            throw new InvalidOperationException("Loan is not active");

        loan.Return();
        await _repository.UpdateAsync(loan, cancellationToken);

        var incremented = await _catalogClient.IncrementAvailabilityAsync(loan.BookId, cancellationToken);
        if (!incremented)
            _logger.LogWarning("Failed to increment book availability for book {BookId}", loan.BookId);

        return MapToDto(loan);
    }

    private static LoanDto MapToDto(Loan loan) => new(
        loan.Id,
        loan.UserId,
        loan.BookId,
        loan.BookTitle,
        loan.UserEmail,
        loan.LoanDate,
        loan.DueDate,
        loan.ReturnDate,
        (BookHub.Shared.DTOs.LoanStatus)(int)loan.Status,
        loan.IsOverdue ? loan.CalculatePenalty() : loan.PenaltyAmount
    );
}
