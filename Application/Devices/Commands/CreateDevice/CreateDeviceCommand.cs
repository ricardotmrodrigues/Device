using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Enums;

namespace Application.Devices.Commands.CreateDevice;

public record CreateDeviceCommand(
    string Name,
    string Brand,
    DeviceStatus State = DeviceStatus.Available
) : ICommand<DeviceDto>;
