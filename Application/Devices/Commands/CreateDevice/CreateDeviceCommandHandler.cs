using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Devices.Commands.CreateDevice;

public class CreateDeviceCommandHandler : ICommandHandler<CreateDeviceCommand, DeviceDto>
{
    private readonly IDeviceEntityRepository _repository;

    public CreateDeviceCommandHandler(IDeviceEntityRepository repository)
    {
        _repository = repository;
    }

    // create device command handler
    public async Task<DeviceDto> HandleAsync(CreateDeviceCommand command, CancellationToken cancellationToken = default)
    {
        var device = new DeviceEntity
        {
            Name = command.Name,
            Brand = command.Brand,
            State = command.State,
            CreationTime = DateTime.UtcNow
        };

        var created = await _repository.AddDeviceAsync(device, cancellationToken);

        return new DeviceDto(created.Id, created.Name, created.Brand, created.State, created.CreationTime);
    }
}
