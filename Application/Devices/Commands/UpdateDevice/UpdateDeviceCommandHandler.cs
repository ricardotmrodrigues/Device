using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Contracts;
using Domain.Enums;

namespace Application.Devices.Commands.UpdateDevice;

public class UpdateDeviceCommandHandler : ICommandHandler<UpdateDeviceCommand, DeviceDto>
{
    private readonly IDeviceEntityRepository _repository;

    public UpdateDeviceCommandHandler(IDeviceEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeviceDto> HandleAsync(UpdateDeviceCommand command, CancellationToken cancellationToken = default)
    {
        var device = await _repository.GetDeviceByIdAsync(command.Id, cancellationToken)
            ?? throw new Exception($"Device with id '{command.Id}' was not found.");

        if (device.State == DeviceStatus.InUse)
        {
            if (command.Name is not null && device.Name != command.Name) 
            {
                throw new Exception("Name cannot be updated when device is in use.");
            }
            if (command.Brand is not null && device.Brand != command.Brand)
            {
                throw new Exception("Brand cannot be updated when device is in use.");
            }
        }

        if (command.Name is not null) device.Name = command.Name;
        if (command.Brand is not null) device.Brand = command.Brand;
        if (command.State is not null) device.State = command.State.Value;

        var updated = await _repository.UpdateDeviceAsync(device, cancellationToken);

        return new DeviceDto(updated.Id, updated.Name, updated.Brand, updated.State, updated.CreationTime);
    }
}
