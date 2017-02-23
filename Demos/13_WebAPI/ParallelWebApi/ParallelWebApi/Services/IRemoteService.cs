using HelloWebApi.Models;
using System.Threading.Tasks;

namespace HelloWebApi.Services
{
    public interface IRemoteService
    {
        Task<ServiceResult> GetDataAsync(int id);
    }
}
