using Microsoft.JSInterop;
using System.Text.Json;

namespace CompanyBlazor5.Client.Services
{
    public interface ILocalStorageService
    {
        Task<T> GetItem<T>(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<T> GetItem<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (json == null)
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task SetItem<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public async Task SaveStringArrayAsync(string key, string[] values)
        {
            await SetItem(key, values == null ? "" : string.Join('\0', values));
        }

        public async Task<string[]> GetStringArrayAsync(string key)
        {
            var data = await GetItem<string>(key);
            if (!string.IsNullOrEmpty(data))
                return data.Split('\0');
            return null;
        }
    }
}