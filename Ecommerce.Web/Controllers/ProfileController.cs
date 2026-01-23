using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Users;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using Ecommerce.Infrastructure.Interfaces;
using Ecommerce.Web.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Ecommerce.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileClient _profileService;
        private readonly IPhotoService _photoService;
        private readonly ILogger<ProfileController> _logger;
        private readonly ICookieTokenService _cookieTokenService;

        public ProfileController(IProfileClient profileService, ILogger<ProfileController> logger, ICookieTokenService cookieTokenService, IPhotoService photoService)
        {
            _profileService = profileService;
            _logger = logger;
            _cookieTokenService = cookieTokenService;
            _photoService = photoService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            // Check if access token exists
            var token = _cookieTokenService.GetAccessToken();

            if(token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _profileService.GetProfileAsync();
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Message);
                return RedirectToAction("Login", "Auth");
            }
            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var result = await _profileService.GetProfileForEditAsync();
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Message);
                return RedirectToAction("Login", "Auth");
            }
            var model = result.Value;
            model.Address ??= new AddressRequest();
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> EditProfile([FromForm]UpdateProfileRequest request)
        {
            try
            {
                if(request.Avatar != null || request.Avatar.Length > 0)
                {
                    if(!string.IsNullOrEmpty(request.AvatarUrl))
                    {
                        await _photoService.DeletePhotoAsync(request.AvatarUrl);
                    }
                    var photoResult = await _photoService.AddPhotoAsync(request.Avatar);
                    if(photoResult.Error != null)
                    {
                        TempData["Error"] = "Lỗi tải ảnh lên. Vui lòng thử lại.";
                        return View(request);
                    }
                    var photoUrl = photoResult.SecureUrl.ToString();
                    request.AvatarUrl = photoUrl;
                }
                var result = await _profileService.UpdateProfileAsync(request);
                if (!result.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.Error.Message);
                    return View(request);
                }
                TempData["Success"] = "Cập nhật thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật hồ sơ: " + ex.Message);
                return View(request);
            }

        }
        
    }
}
