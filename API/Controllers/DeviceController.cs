using API.DTOs.Requests;
using Application.CQRS;
using Application.Devices.Commands.CreateDevice;
using Application.Devices.Commands.DeleteDevice;
using Application.Devices.Commands.UpdateDevice;
using Application.Devices.DTOs;
using Application.Devices.Queries.GetDeviceById;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : APIBaseController<DeviceController>
    {
        public DeviceController(ILogger<DeviceController> logger, IDispatcher dispatcher) : base(logger, dispatcher)
        {
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDto>> GetDevice(int id, CancellationToken ct = default)
        {
            var query = new GetDeviceByIdQuery(id);
            var result = await _dispatcher.QueryAsync<DeviceDto?>(query, ct);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<DeviceDto>> CreateDevice([FromBody] CreateDeviceRequest request, CancellationToken ct = default)
        {
            var command = new CreateDeviceCommand(request.Name, request.Brand, request.State);
            var result = await _dispatcher.SendAsync<CreateDeviceCommand, DeviceDto>(command, ct);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DeviceDto>> UpdateDevice(int id, [FromBody] UpdateDeviceRequest request, CancellationToken ct = default)
        {
            var command = new UpdateDeviceCommand(id, request.Name, request.Brand, request.State);
            var result = await _dispatcher.SendAsync<UpdateDeviceCommand, DeviceDto>(command, ct);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<DeviceDto>> PartialUpdateDevice(int id, [FromBody] PartialUpdateDeviceRequest request, CancellationToken ct = default)
        {
            var command = new UpdateDeviceCommand(id, request.Name, request.Brand, request.State);
            var result = await _dispatcher.SendAsync<UpdateDeviceCommand, DeviceDto>(command, ct);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id, CancellationToken ct = default)
        {
            var command = new DeleteDeviceCommand(id);
            await _dispatcher.SendAsync<DeleteDeviceCommand>(command, ct);
            return NoContent();
        }
    }
}
