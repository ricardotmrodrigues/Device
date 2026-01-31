using Domain.Enums;

namespace API.DTOs.Requests;

public record CreateDeviceRequest(
    string Name,
    string Brand,
    DeviceStatus State
);