using Ecommerce.Application.DTOs.ModelsRequest.Address;
using Ecommerce.Web.Interface;
using Ecommerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private readonly IAddressClient _addressClient;

        public AddressController(IAddressClient addressClient)
        {
            _addressClient = addressClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _addressClient.GetAddressesAsync();
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new List<UserAddressViewModel>());
            }
            return View(result.Value);
        }

        [HttpGet("address/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("address/create")]
        public async Task<IActionResult> Create(CreateAddressRequest request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _addressClient.CreateAddressAsync(request);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(request);
            }
            TempData["Success"] = "Thêm địa chỉ thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("address/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _addressClient.GetAddressesAsync();
            if (!result.IsSuccess || result.Value == null)
            {
                TempData["Error"] = "Không tìm thấy địa chỉ";
                return RedirectToAction(nameof(Index));
            }
            var address = result.Value.FirstOrDefault(x => x.Id == id);
            if (address == null)
            {
                TempData["Error"] = "Không tìm thấy địa chỉ";
                return RedirectToAction(nameof(Index));
            }
            var request = new UpdateAddressRequest
            {
                Id = address.Id,
                Street = address.Street,
                Ward = address.Ward,
                District = address.District,
                City = address.City,
                Province = address.Province,
                IsDefault = address.IsDefault
            };
            return View(request);
        }

        [HttpPost("address/edit/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateAddressRequest request)
        {
            if (!ModelState.IsValid) return View(request);
            request.Id = id;
            var result = await _addressClient.UpdateAddressAsync(request);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(request);
            }
            TempData["Success"] = "Cập nhật địa chỉ thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("address/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _addressClient.GetAddressesAsync();
            if (!result.IsSuccess || result.Value == null)
            {
                TempData["Error"] = "Không tìm thấy địa chỉ";
                return RedirectToAction(nameof(Index));
            }
            var address = result.Value.FirstOrDefault(x => x.Id == id);
            if (address == null)
            {
                TempData["Error"] = "Không tìm thấy địa chỉ";
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        [HttpPost("address/delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _addressClient.DeleteAddressAsync(id);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] = "Xóa địa chỉ thành công";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("address/set-default/{id}")]
        public async Task<IActionResult> SetDefault(int id)
        {
            var result = await _addressClient.SetDefaultAddressAsync(id);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] = "Đã đặt làm địa chỉ mặc định";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
