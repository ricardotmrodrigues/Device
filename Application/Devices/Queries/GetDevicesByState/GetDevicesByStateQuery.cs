using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Enums;

namespace Application.Devices.Queries.GetDevicesByState;

public record GetDevicesByStateQuery(DeviceStatus State) : IQuery<IEnumerable<DeviceDto>>;
