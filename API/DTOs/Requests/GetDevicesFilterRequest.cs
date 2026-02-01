using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Requests;

public record GetDevicesFilterRequest(
    [StringLength(100)] string? Brand = null,
    DeviceStatus? State = null,
    [StringLength(200)] string? Name = null
);