using Ecommerce.Web.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var profile = await _profileService.GetProfileAsync();
            if (profile.IsFailure)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View(profile.Value);
        }
    }
}
