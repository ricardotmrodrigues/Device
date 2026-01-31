using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Contracts;
using Domain.Enums;

namespace Application.Devices.Queries.GetDevicesByState;

public class GetDevicesByStateQueryHandler : IQueryHandler<GetDevicesByStateQuery, IEnumerable<DeviceDto>>
{
    private readonly IDeviceRepository _repository;

    public GetDevicesByStateQueryHandler(IDeviceRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DeviceDto>> HandleAsync(GetDevicesByStateQuery query, CancellationToken cancellationToken = default)
    {
        var devices = await _repository.GetAllDevicesAsync(
            d => d.State == query.State,
            cancellationToken);

        return devices.Select(d => new DeviceDto(d.Id, d.Name, d.Brand, d.State, d.CreationTime));
    }
}
