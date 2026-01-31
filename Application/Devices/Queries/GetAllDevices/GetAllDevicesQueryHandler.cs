using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Contracts;

namespace Application.Devices.Queries.GetAllDevices;

public class GetAllDevicesQueryHandler : IQueryHandler<GetAllDevicesQuery, IEnumerable<DeviceDto>>
{
    private readonly IDeviceEntityRepository _repository;

    public GetAllDevicesQueryHandler(IDeviceEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DeviceDto>> HandleAsync(GetAllDevicesQuery query, CancellationToken cancellationToken = default)
    {
        var devices = await _repository.GetAllDevicesAsync(cancellationToken: cancellationToken);

        return devices.Select(d => new DeviceDto(d.Id, d.Name, d.Brand, d.State, d.CreationTime));
    }
}
