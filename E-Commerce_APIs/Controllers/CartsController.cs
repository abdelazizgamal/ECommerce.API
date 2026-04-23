using ECommerce.BLL;
using ECommerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CartsController : ControllerBase
{
    private readonly ICartManager _cartManager;

    public CartsController(ICartManager cartManager)
    {
        _cartManager = cartManager;
    }

    private string? GetUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpGet]
    public async Task<ActionResult<GeneralResult<CartReadDTO>>> GetCartAsync()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var result = await _cartManager.GetCartAsync(userId);
        return Ok(result);
    }

    [HttpPost("items")]
    public async Task<ActionResult<GeneralResult<CartReadDTO>>> AddToCartAsync([FromBody] AddToCartDTO dto)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var result = await _cartManager.AddToCartAsync(userId, dto);

        if (!result.Success)
            return result.Errors is null ? NotFound(result) : BadRequest(result);

        return Ok(result);
    }

    [HttpPut("items")]
    public async Task<ActionResult<GeneralResult<CartReadDTO>>> UpdateCartItemAsync([FromBody] UpdateCartItemDTO dto)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var result = await _cartManager.UpdateCartItemAsync(userId, dto);

        if (!result.Success)
            return result.Errors is null ? NotFound(result) : BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("items/{productId:int}")]
    public async Task<ActionResult<GeneralResult<CartReadDTO>>> RemoveCartItemAsync(int productId)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var result = await _cartManager.RemoveCartItemAsync(userId, productId);

        if (!result.Success)
            return result.Errors is null ? NotFound(result) : BadRequest(result);

        return Ok(result);
    }
}