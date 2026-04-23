using ECommerce.BLL;
using ECommerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderManager _orderManager;

    public OrdersController(IOrderManager orderManager)
    {
        _orderManager = orderManager;
    }

    private string? GetUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpPost]
    public async Task<ActionResult<GeneralResult<OrderReadDTO>>> CheckoutAsync([FromBody] CreateOrderDTO? dto)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var result = await _orderManager.CreateOrderAsync(userId, dto);

        if (!result.Success)
        {
            if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return NotFound(result);

            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<GeneralResult<IEnumerable<OrderReadDTO>>>> GetMyOrdersAsync()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var result = await _orderManager.GetMyOrdersAsync(userId);
        return Ok(result);
    }

    [HttpGet("all")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<GeneralResult<IEnumerable<OrderReadDTO>>>> GetAllOrdersAsync()
    {
        var result = await _orderManager.GetAllOrdersAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GeneralResult<OrderReadDTO>>> GetOrderByIdAsync(int id)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var result = await _orderManager.GetOrderByIdAsync(userId, id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
