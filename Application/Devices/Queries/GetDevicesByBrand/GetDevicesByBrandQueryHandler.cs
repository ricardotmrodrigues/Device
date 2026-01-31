using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Common;
using Domain.Contracts;

namespace Application.Devices.Queries.GetDevicesByBrand;

public class GetDevicesByBrandQueryHandler : IQueryHandler<GetDevicesByBrandQuery, IEnumerable<DeviceDto>>
{
    private readonly IDeviceEntityRepository _repository;

    public GetDevicesByBrandQueryHandler(IDeviceEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DeviceDto>> HandleAsync(GetDevicesByBrandQuery query, CancellationToken cancellationToken = default)
    {
        var pagedResult = await _repository.GetDevicesPagedAsync(
            predicate: d => d.Brand.ToLower() == query.Brand.ToLower(),
            pagination: new PaginationParameters(PageNumber: 1, PageSize: int.MaxValue),
            cancellationToken: cancellationToken);

        return pagedResult.Items.Select(d => new DeviceDto(d.Id, d.Name, d.Brand, d.State, d.CreationTime));
    }
}
