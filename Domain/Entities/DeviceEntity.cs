using Domain.Enums;

namespace Domain.Entities;
public class DeviceEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public DeviceStatus State { get; set; }
    public DateTime CreationTime { get; set; }
}
