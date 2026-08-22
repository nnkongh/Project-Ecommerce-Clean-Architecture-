using Ecommerce.Application.Common.Command.AuthenticationExternal;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Domain.Enum;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using Ecommerce.WebApi.ViewModels.AuthView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
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
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthenticationClient authClient,
            IMediator mediator,
            SignInManager<AppUser> signinManager,
            IPrincipalFactory principalFactory,
            ILogger<AuthController> logger,
            ICookieTokenService cookieTokenService,
            ITokenService tokenService)
        {
            _authClient = authClient;
            _mediator = mediator;
            _signinManager = signinManager;
            _principalFactory = principalFactory;
            _logger = logger;
            _cookieTokenService = cookieTokenService;
            _tokenService = tokenService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null, string expired = null)
        {
            ViewData["HideAuthHeader"] = true;
            ViewData["ReturnUrl"] = returnUrl;
            if (expired == "true")
            {
                TempData["ErrorMessage"] = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.";
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loginResult = await _authClient.LoginAsync(model);
            if (!loginResult.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Đăng nhập thất bại.");
                return View(model);
            }

            var token = loginResult.Value.AccessToken;
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(string.Empty, "Token không hợp lệ.");
                return View(model);
            }
            _cookieTokenService.SetTokenInsideCookie(loginResult.Value);

            var result = await _signinManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
                return RedirectToAction("Index", "Category");
            return View(model);
        }


        [HttpGet("external-login")]
        public IActionResult ExternalLogin([FromQuery] string provider, [FromQuery] string redirectUri)
        {
            redirectUri = redirectUri ?? Url.Content("~/");

            var callback = Url.Action(nameof(ExternalLoginCallback), "Auth", new { redirectUri = redirectUri }, Request.Scheme);

            var properties = _signinManager.ConfigureExternalAuthenticationProperties(provider, callback);

            return Challenge(properties, provider);
        }
        [HttpGet("login-callback")]
        public async Task<IActionResult> ExternalLoginCallback([FromQuery] string? redirectUri, [FromQuery] string? remoteError = null)
        {
            if (!string.IsNullOrEmpty(remoteError))
            {
                TempData["ErrorMessage"] = $"Lỗi từ nhà cung cấp đăng nhập: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var info = await _signinManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["ErrorMessage"] = "Không thể lấy thông tin đăng nhập ngoài.";
                return RedirectToAction(nameof(Login));
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Không lấy được email từ tài khoản Google.";
                return RedirectToAction(nameof(Login));
            }

            var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email;

            try
            {
                var command = new ExternalLoginCommand
                {
                    info = new ExternalUserInfo
                    {
                        Provider = info!.LoginProvider,
                        ProviderKey = info.ProviderKey,
                        Email = email,
                        Name = name
                    }
                };

                var result = await _mediator.Send(command);

                if (!result.IsSuccess || result.User is null)
                {
                    TempData["ErrorMessage"] = string.IsNullOrEmpty(result.ErrorMessage)
                        ? "Đăng nhập bằng Google thất bại."
                        : result.ErrorMessage;
                    return RedirectToAction(nameof(Login));
                }

                // Cap JWT truc tiep qua TokenService (da dang ky trong Web), khong qua HTTP endpoint nao
                var token = await _tokenService.CreateToken(result.User, populateExp: true);
                _cookieTokenService.SetTokenInsideCookie(token);

                // Sign-in bang user that tu Identity DB, khong dung AppUser map tu DTO
                var appUser = await _signinManager.UserManager.FindByIdAsync(result.User.Id);
                if (appUser == null)
                {
                    TempData["ErrorMessage"] = "Tài khoản chưa được khởi tạo đúng cách.";
                    return RedirectToAction(nameof(Login));
                }
                await _signinManager.SignInAsync(appUser, isPersistent: false);

                // Khong dua token vao URL (tranh lo vao browser history / server logs)
                return Redirect(string.IsNullOrWhiteSpace(redirectUri) ? "~/Category" : redirectUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "External login callback failed");
                TempData["ErrorMessage"] = "Đăng nhập bằng Google thất bại. Vui lòng thử lại.";
                return RedirectToAction(nameof(Login));
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewData["HideAuthHeader"] = true;
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
            try
            {
                await _authClient.LogoutAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Revoke refresh token failed, continuing local logout");
            }

            await _signinManager.SignOutAsync();
            _cookieTokenService.RemoveTokenFromCookie();
            return RedirectToAction("Login", "Auth");
        }
    }
}
