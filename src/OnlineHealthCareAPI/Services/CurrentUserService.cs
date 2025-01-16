using Application.Common.Interfaces;
using Application.Interfaces.Services.CurrentUserService;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIpHelperService _ipHelperService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IIpHelperService ipHelperService)
        {
            _httpContextAccessor = httpContextAccessor;
            _ipHelperService = ipHelperService;
            DeviceToken = _httpContextAccessor.HttpContext?.Request?.Headers["Device-Token"].ToString();
            UserAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
        }
        /// <summary>
        /// Current userId
        /// </summary>
        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public List<string> Roles => _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

        public string IpAddress => _ipHelperService.GetPublicIPAddress();

        public string DeviceToken { get; }
        public string UserAgent { get; }
    }
}
