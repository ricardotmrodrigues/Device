using System;
using Domain.Entities;
using Domain.Enums;

namespace DeviceTestProject.Helpers;

public static class DeviceUnitTestHelper
{
    public static DeviceEntity CreateTestDevice(int id = 1, string name = "Test Device", string brand = "Test Brand", DeviceStatus state = DeviceStatus.Avaliable)
    {
        return new DeviceEntity
        {
            Id = id,
            Name = name,
            Brand = brand,
            State = state,
            CreationTime = DateTime.UtcNow
        };
    }

}
