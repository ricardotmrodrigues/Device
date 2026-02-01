using Domain.Enums;

namespace API.DTOs.Requests;

public record GetDevicesFilterRequest(
    string? Brand = null,
    DeviceStatus? State = null,
    string? Name = null
);