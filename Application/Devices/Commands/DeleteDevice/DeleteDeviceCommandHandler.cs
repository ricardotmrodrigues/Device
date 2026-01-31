using Application.CQRS;
using Application.Exceptions;
using Domain.Contracts;
using Domain.Enums;

namespace Application.Devices.Commands.DeleteDevice;

public class DeleteDeviceCommandHandler : ICommandHandler<DeleteDeviceCommand>
{
    private readonly IDeviceRepository _repository;

    public DeleteDeviceCommandHandler(IDeviceRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(DeleteDeviceCommand command, CancellationToken cancellationToken = default)
    {
        var device = await _repository.GetDeviceByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException("Device", command.Id);

        if (device.State == DeviceStatus.InUse)
        {
            throw new BusinessRuleException("Cannot delete a device that is in use.");
        }

        await _repository.DeleteDeviceAsync(command.Id, cancellationToken);
    }
}
