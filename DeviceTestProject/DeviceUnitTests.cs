using Application.Devices.Commands.CreateDevice;
using Application.Devices.Commands.UpdateDevice;
using Application.Devices.Commands.DeleteDevice;
using Application.Devices.Queries.GetDeviceById;
using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Moq;
using DeviceTestProject.Helpers;

namespace DeviceTestProject;

[TestClass]
public sealed class DeviceUnitTests
{
    private Mock<IDeviceEntityRepository> _mockRepository = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IDeviceEntityRepository>();
    }

    [TestMethod]
    public async Task CreateDevice_ShouldReturnCorrectDeviceDto_WhenDeviceIsCreatedSuccessfully()
    {
        // Arrange
        var command = new CreateDeviceCommand("iPhone 15", "Apple", DeviceStatus.Available);
        var expectedDevice = DeviceUnitTestHelper.CreateTestDevice(1, "iPhone 15", "Apple", DeviceStatus.Available);
        
        _mockRepository
            .Setup(r => r.AddDeviceAsync(It.IsAny<DeviceEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDevice);

        var handler = new CreateDeviceCommandHandler(_mockRepository.Object);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert

        _mockRepository.Verify(r => r.AddDeviceAsync(It.IsAny<DeviceEntity>(), It.IsAny<CancellationToken>()), Times.Once);

        Assert.IsNotNull(result);
        Assert.AreEqual("iPhone 15", result.Name);
        Assert.AreEqual("Apple", result.Brand);
        Assert.AreEqual(DeviceStatus.Available, result.State);
        Assert.IsTrue(DateTime.Now.AddMinutes(-1) < result.CreationTime && result.CreationTime <= DateTime.Now);
    }

    [TestMethod]
    public async Task UpdateDevice_ShouldReturnUpdatedDevice_WhenDeviceExists()
    {
        // Arrange
        var existingDevice = DeviceUnitTestHelper.CreateTestDevice(1, "Old Name", "Old Brand", DeviceStatus.Inactive);
        var updatedDevice = DeviceUnitTestHelper.CreateTestDevice(1, "New Name", "New Brand", DeviceStatus.Available);
        var command = new UpdateDeviceCommand(1, "New Name", "New Brand", DeviceStatus.Available);

        _mockRepository
            .Setup(r => r.GetDeviceByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingDevice);

        _mockRepository
            .Setup(r => r.UpdateDeviceAsync(It.IsAny<DeviceEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedDevice);

        var handler = new UpdateDeviceCommandHandler(_mockRepository.Object);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert

        _mockRepository.Verify(r => r.UpdateDeviceAsync(It.IsAny<DeviceEntity>(), It.IsAny<CancellationToken>()), Times.Once);

        Assert.IsNotNull(result);
        Assert.AreEqual("New Name", result.Name);
        Assert.AreEqual("New Brand", result.Brand);
        Assert.AreEqual(DeviceStatus.Available, result.State);
    }

    [TestMethod]
    public async Task DeleteDevice_ShouldCallRepository_WhenDeviceExists()
    {
        // Arrange
        var existingDevice = DeviceUnitTestHelper.CreateTestDevice(1, "Test Device", "Test Brand", DeviceStatus.Available);
        var command = new DeleteDeviceCommand(1);
        var handler = new DeleteDeviceCommandHandler(_mockRepository.Object);

        _mockRepository
            .Setup(r => r.GetDeviceByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingDevice);

        _mockRepository
            .Setup(r => r.DeleteDeviceAsync(It.IsAny<DeviceEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.DeleteDeviceAsync(It.IsAny<DeviceEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteDevice_ShouldThrowException_WhenDeviceIsInUse()
    {
        // Arrange
        var deviceInUse = DeviceUnitTestHelper.CreateTestDevice(1, "Test Device", "Test Brand", DeviceStatus.InUse);
        var command = new DeleteDeviceCommand(1);
        var handler = new DeleteDeviceCommandHandler(_mockRepository.Object);

        _mockRepository
            .Setup(r => r.GetDeviceByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(deviceInUse);

        // Act
        Exception? exception = null;
        try
        {
            await handler.HandleAsync(command, CancellationToken.None);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        // Assert
        Assert.IsNotNull(exception);
        StringAssert.Contains(exception.Message, "currently in use");
        _mockRepository.Verify(r => r.DeleteDeviceAsync(It.IsAny<DeviceEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task DeleteDevice_ShouldThrowException_WhenDeviceDoesNotExist()
    {
        // Arrange
        var command = new DeleteDeviceCommand(999);
        var handler = new DeleteDeviceCommandHandler(_mockRepository.Object);

        _mockRepository
            .Setup(r => r.GetDeviceByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DeviceEntity?)null);

        // Act
        Exception? exception = null;
        try
        {
            await handler.HandleAsync(command, CancellationToken.None);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        // Assert
        
        _mockRepository.Verify(r => r.DeleteDeviceAsync(It.IsAny<DeviceEntity>(), It.IsAny<CancellationToken>()), Times.Never);

        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public async Task GetDeviceById_ShouldReturnDevice_WhenDeviceExists()
    {
        // Arrange
        var expectedDevice = DeviceUnitTestHelper.CreateTestDevice(1, "iPhone 15", "Apple", DeviceStatus.Available);
        var query = new GetDeviceByIdQuery(1);

        _mockRepository
            .Setup(r => r.GetDeviceByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDevice);

        var handler = new GetDeviceByIdQueryHandler(_mockRepository.Object);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.GetDeviceByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);

        Assert.AreEqual(1, result!.Id);
        Assert.AreEqual("iPhone 15", result.Name);
        Assert.AreEqual("Apple", result.Brand);
        Assert.AreEqual(DeviceStatus.Available, result.State);
    }

    [TestMethod]
    public async Task GetDeviceById_ShouldReturnNull_WhenDeviceDoesNotExist()
    {
        // Arrange
        var query = new GetDeviceByIdQuery(999);

        _mockRepository
            .Setup(r => r.GetDeviceByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DeviceEntity?)null);

        var handler = new GetDeviceByIdQueryHandler(_mockRepository.Object);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.IsNull(result);
    }
}
