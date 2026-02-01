using Application.CQRS;
using Domain.Contracts;
using Domain.Enums;

namespace Application.Devices.Commands.DeleteDevice;

public class DeleteDeviceCommandHandler : ICommandHandler<DeleteDeviceCommand>
{
    private readonly IDeviceEntityRepository _repository;

    public DeleteDeviceCommandHandler(IDeviceEntityRepository repository)
    {
        _repository = repository;
    }

    // delete device command handler
    public async Task HandleAsync(DeleteDeviceCommand command, CancellationToken cancellationToken = default)
    {
        var device = await _repository.GetDeviceByIdAsync(command.Id, cancellationToken)
            ?? throw new Exception($"Device with id '{command.Id}' was not found.");

        //if device is in use, throw exception
        if (device.State == DeviceStatus.InUse)
        {
            throw new Exception("Cannot delete a device that is in use.");
        }

        await _repository.DeleteDeviceAsync(command.Id, cancellationToken);
    }
}
