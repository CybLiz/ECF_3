using BookHub.Shared.DTOs;
using BookHubGateway.Infrastructure.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace BookHubGateway.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoanGatewayController : ControllerBase
    {
        private readonly RestClient<LoanDto, CreateLoanDto> _loanClient;

        public LoanGatewayController()
        {
            _loanClient = new RestClient<LoanDto, CreateLoanDto>("http://localhost:5003/api/loans");
        }

        [HttpGet]
        public async Task<List<LoanDto>> GetAll()
        {
            return await _loanClient.GetListRequest("");
        }

        [HttpGet("{id:guid}")]
        public async Task<LoanDto> GetById(Guid id)
        {
            return await _loanClient.GetRequest($"/{id}");
        }

        [HttpPost]
        public async Task<LoanDto> Create([FromBody] CreateLoanDto dto)
        {
            var token = Request.Headers["Authorization"].ToString();
            _loanClient.SetAuthorizationHeader(token);
            return await _loanClient.PostRequest("", dto);
        }

        [HttpGet("overdue")]
        public async Task<List<LoanDto>> GetOverdue()
        {
            return await _loanClient.GetListRequest("/overdue");
        }

        //  /loans/{id}/return sans body
        [HttpPut("{id:guid}/return")]
        public async Task<LoanDto> Return(Guid id)
        {
            return await _loanClient.PostRequest($"/{id}/return");
        }


        [HttpGet("user/{userId:guid}")]
        public async Task<List<LoanDto>> GetByUserId(Guid userId)
        {
            return await _loanClient.GetListRequest($"/user/{userId}");
        }



    }
}
