using Application.Interfaces.Services.CurrentUserService;
using Services;

namespace kutumbaAPI
{
    public static class ConfigureServices
    {
        public static void AddWebUIServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();


        }
    }
}
