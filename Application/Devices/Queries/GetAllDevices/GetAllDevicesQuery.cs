using Application.CQRS;
using Application.Devices.DTOs;

namespace Application.Devices.Queries.GetAllDevices;

public record GetAllDevicesQuery : IQuery<IEnumerable<DeviceDto>>;
