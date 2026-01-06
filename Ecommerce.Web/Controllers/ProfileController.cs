using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.User;
using Ecommerce.Domain.Models;
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
        private readonly ILogger<ProfileController> _logger;
        public ProfileController(IProfileClient profileService, ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var profile = await _profileService.GetProfileAsync();
            if (!profile.IsSuccess)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View(profile.Value);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var result = await _profileService.GetProfileForEditAsync();
            if (!result.IsSuccess)
            {
                return RedirectToAction("Login", "Auth");
            }
            var model = result.Value;
            model.Address ??= new AddressRequest();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UpdateProfileRequest request)
        {
            _logger.LogWarning("Edit profile call");
            var result = await _profileService.UpdateProfileAsync(request);
            if(!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error.Message);
                return View(request);
            }
            TempData["Success"] = "Cập nhật thành công";
            return RedirectToAction(nameof(Index));

        }
        
    }
}
