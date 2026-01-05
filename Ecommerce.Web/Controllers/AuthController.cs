using AutoMapper;
using Ecommerce.Application.Common.Command.AuthenticationExternal;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Ecommerce.WebApi.ViewModels.AuthView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using System.Runtime.CompilerServices;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Web.Interface;
using Ecommerce.Web.ViewModels;

namespace Ecommerce.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticationClient _authClient;
        private readonly IMediator _mediator;
        private readonly IPrincipalFactory _principalFactory;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly ICookieTokenService _cookieTokenService;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthenticationClient authClient,
            IMediator mediator,
            HttpClient httpClient,
            SignInManager<AppUser> signinManager,
            IPrincipalFactory principalFactory,
            ILogger<AuthController> logger,
            IMapper mapper,
            ICookieTokenService cookieTokenService)
        {
            _authClient = authClient;
            _mediator = mediator;
            _httpClient = httpClient;
            _signinManager = signinManager;
            _principalFactory = principalFactory;
            _logger = logger;
            _mapper = mapper;
            _cookieTokenService = cookieTokenService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authClient.LoginAsync(model);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Đăng nhập thất bại.");
                return View(model);
            }
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Đăng nhập thất bại.");
                return View(model);
            }

            var token = result.Value.AccessToken;
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(string.Empty, "Token không hợp lệ.");
                return View(model);
            }
            _cookieTokenService.SetTokenInsideCookie(result.Value);

            var principal = _principalFactory.CreatePrincipalFromToken(token);
            if (principal == null)
            {
                ModelState.AddModelError(string.Empty, "Token không hợp lệ.");
                return View(model);
            }

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal,
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                });
            return RedirectToAction("Index", "Home");
        }


        [HttpGet("external-login")]
        public IActionResult ExternalLogin([FromQuery] string provider, [FromQuery] string redirectUri)
        {
            redirectUri = redirectUri ?? Url.Content("~/");

            var callback = Url.Action(nameof(ExternalLoginCallback), "Auth", new { redirectUri = redirectUri }, Request.Scheme);

            var properties = _signinManager.ConfigureExternalAuthenticationProperties(provider, callback);

            return Challenge(properties, provider);
        }
        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback([FromQuery] string redirectUri, [FromQuery] string remoteError = null)
        {
            var info = await _signinManager.GetExternalLoginInfoAsync();

            var command = new ExternalLoginCommand
            {
                info = new ExternalUserInfo
                {
                    Provider = info!.LoginProvider,
                    ProviderKey = info.ProviderKey,
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                    Name = info.Principal.FindFirstValue(ClaimTypes.Name)!
                }
            };
            try
            {
                var result = await _mediator.Send(command);

                var tokenResponse = await _httpClient.PostAsJsonAsync("https://localhost:7021/token/create-token", result.User);

                var token = await tokenResponse.Content.ReadFromJsonAsync<TokenModel>();

                var mapped = _mapper.Map<AppUser>(result.User);

                _cookieTokenService.SetTokenInsideCookie(token!);

                await _signinManager.SignInAsync(mapped, isPersistent: false);

                return Redirect($"{redirectUri}#accesstoken={token.AccessToken}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mediator Send failed");
                return StatusCode(500, ex.Message);

            }

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _authClient.RegisterAsync(model);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error?.Message ?? "Đăng ký thất bại");
                return View(model);
            }
            TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            // model.ClientUrl = $"{Request.Scheme}://{Request.Host}";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var response = await _authClient.ForgotPasswordAsync(model);
            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Yêu cầu đặt lại mật khẩu thất bại.");
                return View(model);
            }
            TempData["SuccessMessage"] = "Yêu cầu đặt lại mật khẩu thành công. Vui lòng kiểm tra email của bạn.";
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid reset password link.");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var request = await _authClient.ResetPasswordAsync(model);
            if (!request.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Đặt lại mật khẩu thất bại.");
                return View(model);
            }
            TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công. Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authClient.LogoutAsync();
            await _signinManager.SignOutAsync();
            _cookieTokenService.RemoveTokenFromCookie();
            return RedirectToAction("Index", "Home");
        }
    }
}
