using VelocityShared;

namespace VelocityWeb.Services
{
    public interface IPermissionService
    {
        Task<List<PermissionInfo>> GetPermissionsAsync(int userId);
        Task LoadPermissionsAsync(int userId);
    }
}
