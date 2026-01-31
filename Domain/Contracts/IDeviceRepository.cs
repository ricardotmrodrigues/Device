using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Contracts
{
    public interface IDeviceRepository
    {
        Task<bool> AddDeviceAsync(DeviceEntity device, CancellationToken cancellationToken);
        Task<bool> UpdateDeviceAsync(DeviceEntity device, CancellationToken cancellationToken);
        Task<bool> DeleteDeviceAsync(int deviceId, CancellationToken cancellationToken);
        Task<DeviceEntity?> GetDeviceByIdAsync(int deviceId, CancellationToken cancellationToken);
        Task<IEnumerable<DeviceEntity>> GetAllDevicesAsync(Expression<Func<DeviceEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    }
}
