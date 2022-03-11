using System.Diagnostics;
using JwtAspNetCoreWebApiSample.Helpers;
using JwtAspNetCoreWebApiSample.Models;
using JwtAspNetCoreWebApiSample.Models.Dto;
using JwtAspNetCoreWebApiSample.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAspNetCoreWebApiSample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private static readonly UserEntity UserEntity = new();
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthController(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<UserEntity>> Register(UserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model is not valid");

        Debug.Assert(request.Username != null, "request.Username != null");
        Debug.Assert(request.Password != null, "request.Password != null");

        AuthHelper.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

        UserEntity.Username = request.Username;
        UserEntity.PasswordHash = passwordHash;
        UserEntity.PasswordSalt = passwordSalt;

        return Ok(UserEntity);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        if (UserEntity.Username != request.Username) return BadRequest("User not found");

        Debug.Assert(UserEntity.PasswordSalt != null, "UserEntity.PasswordSalt != null");
        Debug.Assert(UserEntity.PasswordHash != null, "UserEntity.PasswordHash != null");
        Debug.Assert(request.Password != null, "request.Password != null");

        if (!AuthHelper.VerifyPasswordHash(request.Password, UserEntity.PasswordHash, UserEntity.PasswordSalt))
            return BadRequest("Wrong password");

        var token = AuthHelper.CreateUserToken(UserEntity.Username,
            _configuration.GetSection("AppSettings:Secret").Value);
        return Ok(token);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<string>> GetMe()
    {
        var userName = _userService.GetMyName();
        return Ok(userName);
    }
}