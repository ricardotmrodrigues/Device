using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Enums;

namespace Application.Devices.Commands.PartialUpdateDevice;

public record PartialUpdateDeviceCommand(int Id, string? Name, string? Brand, DeviceStatus? State) : ICommand<DeviceDto>;
