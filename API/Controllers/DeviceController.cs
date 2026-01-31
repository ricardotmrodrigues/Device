using API.DTOs.Requests;
using Application.CQRS;
using Application.Devices.Commands.CreateDevice;
using Application.Devices.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : APIBaseController<DeviceController>
    {
        public DeviceController(
            ILogger<DeviceController> logger,
            IDispatcher dispatcher) : base(logger, dispatcher)
        {
        }

        [HttpPost]
        public async Task<ActionResult<DeviceDto>> CreateDevice([FromBody] CreateDeviceRequest request, CancellationToken ct = default)
        {
            var command = new CreateDeviceCommand(request.Name, request.Brand, request.State);
            var result = await _dispatcher.SendAsync<CreateDeviceCommand, DeviceDto>(command, ct);
            return Ok(result);
        }
    }
}
