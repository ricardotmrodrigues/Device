using Domain.Common;
using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Contracts
{
    public interface IDeviceEntityRepository
    {
        // add device entity
        Task<DeviceEntity> AddDeviceAsync(DeviceEntity device, CancellationToken cancellationToken);

        //upadte device entity
        Task<DeviceEntity> UpdateDeviceAsync(DeviceEntity device, CancellationToken cancellationToken);

        // delete device entity
        Task DeleteDeviceAsync(DeviceEntity device, CancellationToken cancellationToken);

        // gets one device by its id
        Task<DeviceEntity?> GetDeviceByIdAsync(int deviceId, CancellationToken cancellationToken);

        // gets a paged result of devices
        Task<PagedResult<DeviceEntity>> GetDevicesPagedAsync(
            Expression<Func<DeviceEntity, bool>>? predicate = null,
            PaginationParameters? pagination = null,
            CancellationToken cancellationToken = default);
    }
}
