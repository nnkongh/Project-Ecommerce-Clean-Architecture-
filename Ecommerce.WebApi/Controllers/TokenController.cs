using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers
{
    [Route("token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<TokenController> _logger;
        public TokenController(ITokenService tokenService, ILogger<TokenController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }
        [HttpPost]
        [Route("create-token")]
        public async Task<IActionResult> CreateToken([FromBody]UserModel user)
        {
            _logger.LogInformation("Creating token..");
            var tokenResult = await _tokenService.CreateToken(user, true);
            return Ok(tokenResult);
        } 
    }
}
