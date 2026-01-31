using Application.CQRS;
using Application.Devices.DTOs;
using Application.Exceptions;
using Domain.Contracts;
using Domain.Enums;

namespace Application.Devices.Commands.UpdateDevice;

public class UpdateDeviceCommandHandler : ICommandHandler<UpdateDeviceCommand, DeviceDto>
{
    private readonly IDeviceRepository _repository;

    public UpdateDeviceCommandHandler(IDeviceRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeviceDto> HandleAsync(UpdateDeviceCommand command, CancellationToken cancellationToken = default)
    {
        var device = await _repository.GetDeviceByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException("Device", command.Id);

        if (device.State == DeviceStatus.InUse)
        {
            if (device.Name != command.Name || device.Brand != command.Brand)
            {
                throw new BusinessRuleException("Name and brand cannot be updated when device is in use.");
            }
        }

        device.Name = command.Name;
        device.Brand = command.Brand;
        device.State = command.State;

        var updated = await _repository.UpdateDeviceAsync(device, cancellationToken);

        return new DeviceDto(updated.Id, updated.Name, updated.Brand, updated.State, updated.CreationTime);
    }
}
