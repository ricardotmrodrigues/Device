namespace Domain.Common.Exceptions;

public class DeviceInUseException : Exception
{
    public int DeviceId { get; }

    public DeviceInUseException(int deviceId, string operation)
        : base($"Cannot {operation} device with id '{deviceId}' because it is currently in use.")
    {
        DeviceId = deviceId;
    }
}
