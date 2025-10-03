using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;

        public TokenController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenDto)
        {
            //var tokenRef = await _authService.CreateRefreshToken(tokenDto);
            return Ok();
        } 
    }
}
