namespace Domain.Common.Exceptions;

public class DeviceNotFoundException : Exception
{
    public int DeviceId { get; }

    public DeviceNotFoundException(int deviceId)
        : base($"Device with id '{deviceId}' was not found.")
    {
        DeviceId = deviceId;
    }
}
