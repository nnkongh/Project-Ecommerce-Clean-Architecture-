using Ecommerce.Web.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.Web.Controllers
{
 //   [Authorize]
    public class ProfileController : Controller
    {
        private readonly IAuthClient _authClient;

        public ProfileController(IAuthClient authClient)
        {
            _authClient = authClient;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var profile = await _authClient.GetProfileAsync();
            return View(profile.Value);
        }
    }
}
