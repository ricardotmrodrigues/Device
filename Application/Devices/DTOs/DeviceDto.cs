using Domain.Enums;

namespace Application.Devices.DTOs;

public record DeviceDto(int Id, string Name, string Brand, DeviceStatus State, DateTime CreationTime);
