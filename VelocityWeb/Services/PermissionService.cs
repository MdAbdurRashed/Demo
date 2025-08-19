using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VelocityShared;
using VelocityWeb.ViewModel;

namespace VelocityWeb.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDistributedCache _cache;
        private readonly string _baseUrl;

        public PermissionService(
            IHttpClientFactory httpClientFactory,
            IDistributedCache cache,
            IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
            _baseUrl = apiSettings.Value.BaseUrl.TrimEnd('/');
        }

        public async Task LoadPermissionsAsync(int userId)
        {
            var client = _httpClientFactory.CreateClient();
            var permissions = await client.GetFromJsonAsync<List<PermissionInfo>>(
                $"{_baseUrl}/api/permission/get-by-user?userId={userId}");

            if (permissions != null)
            {
                var serialized = JsonConvert.SerializeObject(permissions);
                await _cache.SetStringAsync($"permissions:{userId}", serialized,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    });
            }
        }

        public async Task<List<PermissionInfo>> GetPermissionsAsync(int userId)
        {
            var cached = await _cache.GetStringAsync($"permissions:{userId}");
            if (!string.IsNullOrEmpty(cached))
                return JsonConvert.DeserializeObject<List<PermissionInfo>>(cached)!;

            await LoadPermissionsAsync(userId);
            cached = await _cache.GetStringAsync($"permissions:{userId}");
            return JsonConvert.DeserializeObject<List<PermissionInfo>>(cached)!;
        }
    }
}
