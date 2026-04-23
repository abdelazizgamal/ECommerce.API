using ECommerce.API.Services;
using ECommerce.APIs;
using ECommerce.Common;
using ECommerce.DAL;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        JwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<GeneralResult>> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(GeneralResult<RegisterDto>.FailResult("Invalid registration data."));
        }

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser is not null)
            return BadRequest(GeneralResult.FailResult("Email already exists."));

        var user = new AppUser
        {
            UserName = dto.Email.Split('@')[0],
            Email = dto.Email,
            FullName = dto.FullName
        };

        var createResult = await _userManager.CreateAsync(user, dto.Password);
        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => new Common.Error
                    {
                        Code = e.Code,
                        Message = e.Description
                    }).ToList());

            return BadRequest(GeneralResult.FailResult(errors));
        }

        if (!await _roleManager.RoleExistsAsync("User"))
            return BadRequest(GeneralResult.FailResult("Required role 'User' does not exist."));

        await _userManager.AddToRoleAsync(user, "User");

        return Ok(GeneralResult.SuccessResult("Registration successful. Please login."));
    }

    [HttpPost("login")]
    public async Task<ActionResult<GeneralResult<TokenDto>>> Login([FromBody] UserLoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return NotFound(GeneralResult<TokenDto>.NotFound("Invalid email or password."));

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid)
            return BadRequest(GeneralResult<TokenDto>.FailResult("Invalid email or password."));

        var roles = await _userManager.GetRolesAsync(user);
        var tokenDto = _jwtTokenService.CreateToken(user, roles);

        return Ok(GeneralResult<TokenDto>.SuccessResult(tokenDto, "Login successful."));
    }
}