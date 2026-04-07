using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV22T1020040.Admin;
using SV22T1020040.BusinessLayers;
using SV22T1020040.Models.Sales;
using System.Security.Claims;

namespace SV22T1020040.Shop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int? status = null)
        {
            // Lấy CustomerID từ Claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "ShopAccount");
            }
            int customerId = int.Parse(userIdClaim.Value);

            var input = new OrderSearchInput
            {
                Page = page,
                PageSize = 10,
                SearchValue = "",
                Status = status.HasValue ? (OrderStatusEnum)status.Value : null
            };

            // 👉 Dùng phương thức mới để lấy đơn hàng theo customer
            var orders = await SalesDataService.ListOrdersByCustomerAsync(customerId, input);

            ViewBag.CurrentStatus = status;
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "ShopAccount");
            }
            int customerId = int.Parse(userIdClaim.Value);

            var order = await SalesDataService.GetOrderAsync(id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }

            // ⚠️ KIỂM TRA QUYỀN
            if (order.CustomerID != customerId)
            {
                TempData["Error"] = "Bạn không có quyền xem đơn hàng này";
                return RedirectToAction("Index");
            }

            var details = await SalesDataService.ListDetailsAsync(id);
            ViewBag.Details = details;

            return View(order);
        }
    }
}