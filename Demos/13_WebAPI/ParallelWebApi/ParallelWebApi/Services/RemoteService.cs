using System.Threading.Tasks;
using HelloWebApi.Models;

namespace HelloWebApi.Services
{
    public class RemoteService : IRemoteService
    {
        private const int _delay = 500;

        public async Task<ServiceResult> GetDataAsync(int id)
        {
            await Task.Delay(_delay);
            return new ServiceResult { Data = $"Remote Service {id} Restult" };
        }
    }
}
