using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAspNetCoreWebApiSample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("testAdmin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Test1()
    {
        return Ok();
    }

    [HttpGet("testOther")]
    [Authorize(Roles = "Other")]
    public async Task<ActionResult> Test2()
    {
        return Ok();
    }
}