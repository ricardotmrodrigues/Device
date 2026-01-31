using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Contracts;

namespace Application.Devices.Queries.GetDeviceById;

public class GetDeviceByIdQueryHandler : IQueryHandler<GetDeviceByIdQuery, DeviceDto?>
{
    private readonly IDeviceRepository _repository;

    public GetDeviceByIdQueryHandler(IDeviceRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeviceDto?> HandleAsync(GetDeviceByIdQuery query, CancellationToken cancellationToken = default)
    {
        var device = await _repository.GetDeviceByIdAsync(query.Id, cancellationToken);

        return device is null
            ? null
            : new DeviceDto(device.Id, device.Name, device.Brand, device.State, device.CreationTime);
    }
}
