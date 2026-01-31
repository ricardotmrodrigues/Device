using Application.CQRS;
using Application.Devices.DTOs;

namespace Application.Devices.Queries.GetDevicesByBrand;

public record GetDevicesByBrandQuery(string Brand) : IQuery<IEnumerable<DeviceDto>>;
