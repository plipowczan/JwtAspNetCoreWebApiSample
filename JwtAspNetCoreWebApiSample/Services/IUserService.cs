using System.Security.Claims;

namespace JwtAspNetCoreWebApiSample.Services
{
    public interface IUserService
    {
        string? GetMyName();
    }

    class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string? GetMyName()
        {
            if (_contextAccessor.HttpContext != null)
                return _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            return null;
        }
    }
}
