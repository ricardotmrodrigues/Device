using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Common.Exceptions;
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

    // update device command handler
    public async Task<DeviceDto> HandleAsync(UpdateDeviceCommand command, CancellationToken cancellationToken = default)
    {
        var device = await _repository.GetDeviceByIdAsync(command.Id, cancellationToken)
            ?? throw new DeviceNotFoundException(command.Id);

        //if device is in use...
        if (device.State == DeviceStatus.InUse)
        {
            //we cannot update name
            if (command.Name is not null && device.Name != command.Name) 
            {
                throw new DeviceInUseException(command.Id, "update name of");
            }
            //we cannot update brand
            if (command.Brand is not null && device.Brand != command.Brand)
            {
                throw new DeviceInUseException(command.Id, "update brand of");
            }
        }

        //update fields if they are provided
        if (command.Name is not null) device.Name = command.Name;
        if (command.Brand is not null) device.Brand = command.Brand;
        if (command.State is not null) device.State = command.State.Value;

        var updated = await _repository.UpdateDeviceAsync(device, cancellationToken);

        return new DeviceDto(updated.Id, updated.Name, updated.Brand, updated.State, updated.CreationTime);
    }
}
