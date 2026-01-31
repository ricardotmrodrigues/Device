using Application.Devices.DTOs;
using Domain.Enums;

namespace API.DTOs.Responses;

public record DeviceResponse(
    int Id,
    string Name,
    string Brand,
    DeviceStatus State,
    DateTime CreationTime
)
{
    public static DeviceResponse FromDeviceDto(DeviceDto deviceDto)
    {
        return new DeviceResponse(
            deviceDto.Id,
            deviceDto.Name,
            deviceDto.Brand,
            deviceDto.State,
            deviceDto.CreationTime
        );
    }
}