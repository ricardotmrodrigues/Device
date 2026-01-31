using Application.CQRS;

namespace Application.Devices.Commands.DeleteDevice;

public record DeleteDeviceCommand(int Id) : ICommand;
