using BookHub.Shared.DTOs;
using BookHubGateway.Infrastructure.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace BookHubGateway.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookGatewayController : ControllerBase
    {
        private readonly RestClient<BookDto, CreateBookDto> _bookClient;

        public BookGatewayController()
        {
            _bookClient = new RestClient<BookDto, CreateBookDto>("http://localhost:5001/api/books");
        }

        [HttpGet]
        public async Task<List<BookDto>> GetAll()
        {
            return await _bookClient.GetListRequest("");
        }

        [HttpGet("{id:guid}")]
        public async Task<BookDto> GetById(Guid id)
        {
            return await _bookClient.GetRequest($"/{id}");
        }

        [HttpGet("search")]
        public async Task<List<BookDto>> Search([FromQuery] string term)
        {
            return await _bookClient.GetListRequest($"/search?term={term}");
        }

        [HttpGet("category/{category}")]
        public async Task<List<BookDto>> GetByCategory(string category)
        {
            return await _bookClient.GetListRequest($"/category/{category}");
        }

        [HttpPost]
        public async Task<BookDto> Create([FromBody] CreateBookDto dto)
        {
            return await _bookClient.PostRequest("", dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<BookDto> Update(Guid id, [FromBody] UpdateBookDto dto)
        {
            return await _bookClient.PutRequest($"/{id}", dto);
        }

        [HttpPost("{id:guid}/decrement-availability")]
        public async Task<IActionResult> DecrementAvailability(Guid id)
        {
            await _bookClient.PostRequest<object>($"/{id}/decrement-availability", null);
            return Ok();
        }


        [HttpPost("{id:guid}/increment-availability")]
        public async Task<IActionResult> IncrementAvailability(Guid id)
        {
            await _bookClient.PostRequest<object>($"/{id}/increment-availability", null);
            return Ok();
        }
    }
}
