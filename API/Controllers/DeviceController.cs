using API.DTOs.Requests;
using API.DTOs.Responses;
using API.Extensions;
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
        public async Task<ActionResult<DeviceResponse>> GetDevice(int id, CancellationToken ct = default)
        {
            var query = new GetDeviceByIdQuery(id);
            var result = await _dispatcher.QueryAsync<DeviceDto?>(query, ct);

            if (result == null)
                return NotFound();

            return Ok(DeviceResponse.FromDeviceDto(result));
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<DeviceResponse>>> GetDevices([FromBody] GetDevicesFilterRequest request, CancellationToken ct = default)
        {
            var query = request.ToFilterQuery();
            var result = await _dispatcher.QueryAsync<IEnumerable<DeviceDto>>(query, ct);

            var response = result.Select(DeviceResponse.FromDeviceDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<DeviceResponse>> CreateDevice([FromBody] CreateDeviceRequest request, CancellationToken ct = default)
        {
            var command = new CreateDeviceCommand(request.Name, request.Brand, request.State);
            var result = await _dispatcher.SendAsync<CreateDeviceCommand, DeviceDto>(command, ct);
            return Ok(DeviceResponse.FromDeviceDto(result));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<DeviceResponse>> UpdateDevice(int id, [FromBody] PartialUpdateDeviceRequest request, CancellationToken ct = default)
        {
            var command = new UpdateDeviceCommand(id, request.Name, request.Brand, request.State);
            var result = await _dispatcher.SendAsync<UpdateDeviceCommand, DeviceDto>(command, ct);
            return Ok(DeviceResponse.FromDeviceDto(result));
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
