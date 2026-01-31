using Application.CQRS;
using Application.Devices.DTOs;

namespace Application.Devices.Queries.GetDeviceById;

public record GetDeviceByIdQuery(int Id) : IQuery<DeviceDto?>;
