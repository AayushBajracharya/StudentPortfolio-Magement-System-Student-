namespace Application.Interfaces.Services.CurrentUserService
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        List<string> Roles { get; }
        string IpAddress { get; }
    }
}
