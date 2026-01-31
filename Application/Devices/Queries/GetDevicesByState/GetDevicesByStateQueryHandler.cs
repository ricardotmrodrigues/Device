using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Common;
using Domain.Contracts;
using Domain.Enums;

namespace Application.Devices.Queries.GetDevicesByState;

public class GetDevicesByStateQueryHandler : IQueryHandler<GetDevicesByStateQuery, IEnumerable<DeviceDto>>
{
    private readonly IDeviceEntityRepository _repository;

    public GetDevicesByStateQueryHandler(IDeviceEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DeviceDto>> HandleAsync(GetDevicesByStateQuery query, CancellationToken cancellationToken = default)
    {
        var pagedResult = await _repository.GetDevicesPagedAsync(
            predicate: d => d.State == query.State,
            pagination: new PaginationParameters(PageNumber: 1, PageSize: int.MaxValue),
            cancellationToken: cancellationToken);

        return pagedResult.Items.Select(d => new DeviceDto(d.Id, d.Name, d.Brand, d.State, d.CreationTime));
    }
}
