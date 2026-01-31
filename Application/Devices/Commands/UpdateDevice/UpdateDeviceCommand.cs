using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Enums;

namespace Application.Devices.Commands.UpdateDevice;

public record UpdateDeviceCommand(int Id, string Name, string Brand, DeviceStatus State) : ICommand<DeviceDto>;
