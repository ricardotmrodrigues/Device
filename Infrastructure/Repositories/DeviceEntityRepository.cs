using System.Linq.Expressions;
using Domain.Common;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DeviceEntityRepository : IDeviceEntityRepository
{
    private readonly DeviceDBContext _context;

    public DeviceEntityRepository(DeviceDBContext context)
    {
        _context = context;
    }

    public async Task<DeviceEntity> AddDeviceAsync(DeviceEntity device, CancellationToken cancellationToken)
    {
        await _context.Devices.AddAsync(device, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return device;
    }

    public async Task DeleteDeviceAsync(int deviceId, CancellationToken cancellationToken)
    {
        var device = new DeviceEntity { Id = deviceId };
        _context.Devices.Remove(device);
        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task<DeviceEntity?> GetDeviceByIdAsync(int deviceId, CancellationToken cancellationToken)
    {
        return await _context.Devices.FindAsync(new object[] { deviceId }, cancellationToken);
    }

    public async Task<DeviceEntity> UpdateDeviceAsync(DeviceEntity device, CancellationToken cancellationToken)
    {
        _context.Devices.Update(device);
        await _context.SaveChangesAsync(cancellationToken);
        return device;
    }

    public async Task<PagedResult<DeviceEntity>> GetDevicesPagedAsync(
        Expression<Func<DeviceEntity, bool>>? predicate = null,
        PaginationParameters? pagination = null,
        CancellationToken cancellationToken = default)
    {
        pagination ??= new PaginationParameters();

        IQueryable<DeviceEntity> query = _context.Devices;

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip(pagination.Skip)
            .Take(pagination.Take)
            .ToListAsync(cancellationToken);

        return new PagedResult<DeviceEntity>(items, totalCount, pagination.PageNumber, pagination.PageSize);
    }
}
