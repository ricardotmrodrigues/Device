using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Common;
using Domain.Contracts;

namespace Application.Devices.Queries.GetDevicesByFilter;

public class GetDevicesByFilterQueryHandler: IQueryHandler<GetDevicesByFilterQuery, IEnumerable<DeviceDto>>
{
    private readonly IDeviceEntityRepository _repository;

    public GetDevicesByFilterQueryHandler(IDeviceEntityRepository repository)
    {
        _repository = repository;
    }

    // get devices by filter query handler
    public async Task<IEnumerable<DeviceDto>> HandleAsync(GetDevicesByFilterQuery query, CancellationToken cancellationToken = default)
    {
        var pagedResult = await _repository.GetDevicesPagedAsync(
            predicate: query.Predicate,
            pagination: new PaginationParameters(PageNumber: 1, PageSize: int.MaxValue),
            cancellationToken: cancellationToken);

        return pagedResult.Items.Select(d => new DeviceDto(d.Id, d.Name, d.Brand, d.State, d.CreationTime));
    }
}