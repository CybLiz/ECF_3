using BookHub.Shared.DTOs;
using BookHubGateway.Infrastructure.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.Gateway.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserGatewayController : ControllerBase
    {
        private readonly RestClient<UserDto, CreateUserDto> _userRegisterClient;
        private readonly RestClient<UserDto, UpdateUserDto> _userUpdateClient;
        private readonly RestClient<LoginResponseDto, LoginDto> _userLoginClient;
        private readonly RestClient<UserDto, object> _userClient;

        public UserGatewayController()
        {
            var baseUrl = "http://localhost:5002/api/users";

            _userClient = new RestClient<UserDto, object>(baseUrl);
            _userRegisterClient = new RestClient<UserDto, CreateUserDto>($"{baseUrl}/register");
            _userUpdateClient = new RestClient<UserDto, UpdateUserDto>(baseUrl);
            _userLoginClient = new RestClient<LoginResponseDto, LoginDto>($"{baseUrl}/login");
        }

        [HttpGet]
        public async Task<List<UserDto>> GetAll()
        {
            return await _userClient.GetListRequest("");
        }

        [HttpGet("{id:guid}")]
        public async Task<UserDto> GetById(Guid id)
        {
            return await _userClient.GetRequest($"/{id}");
        }

        [HttpPost("register")]
        public async Task<UserDto> Register([FromBody] CreateUserDto dto)
        {
            return await _userRegisterClient.PostRequest("", dto);
        }

        [HttpPost("login")]
        public async Task<LoginResponseDto> Login([FromBody] LoginDto dto)
        {
            return await _userLoginClient.PostRequest("", dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<UserDto> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            return await _userUpdateClient.PostRequest($"/{id}", dto);
        }

        [HttpDelete("{id:guid}")]
        public async Task Delete(Guid id)
        {
            await _userClient.DeleteRequest($"/{id}");
        }
    }
}
