using Domain.Enums;

namespace API.DTOs.Requests;

public record PartialUpdateDeviceRequest(
    string? Name,
    string? Brand,
    DeviceStatus? State
);