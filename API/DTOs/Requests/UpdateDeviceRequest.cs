using Domain.Enums;

namespace API.DTOs.Requests;

public record UpdateDeviceRequest(
    string Name,
    string Brand,
    DeviceStatus State
);