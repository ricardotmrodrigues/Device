using Domain.Common;
using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Contracts
{
    public interface IDeviceEntityRepository
    {
        Task<DeviceEntity> AddDeviceAsync(DeviceEntity device, CancellationToken cancellationToken);
        Task<DeviceEntity> UpdateDeviceAsync(DeviceEntity device, CancellationToken cancellationToken);
        Task DeleteDeviceAsync(int deviceId, CancellationToken cancellationToken);
        Task<DeviceEntity?> GetDeviceByIdAsync(int deviceId, CancellationToken cancellationToken);
        Task<IEnumerable<DeviceEntity>> GetAllDevicesAsync(Expression<Func<DeviceEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task<PagedResult<DeviceEntity>> GetDevicesPagedAsync(
            Expression<Func<DeviceEntity, bool>>? predicate = null,
            PaginationParameters? pagination = null,
            CancellationToken cancellationToken = default);
    }
}
