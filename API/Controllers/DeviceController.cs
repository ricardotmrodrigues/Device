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
        
        /// <summary>
        /// Gets new device by id
        /// </summary>
        /// <param name="id">Device id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The device with supplied id</returns>
        /// <response code="200">Returns the device</response>
        /// <response code="404">If the device was not found</response>
        /// <response code="400">If the request is invalid</response>
        [HttpGet("{id}")]        
        [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeviceResponse>> GetDevice(int id, CancellationToken ct = default)
        {
            var query = new GetDeviceByIdQuery(id);
            var result = await _dispatcher.QueryAsync<DeviceDto?>(query, ct);

            if (result == null)
                return NotFound();

            return Ok(DeviceResponse.FromDeviceDto(result));
        }

        /// <summary>
        /// Gets devices according to filter
        /// </summary>
        /// <param name="request">Filter request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The devices with supplied filters</returns>
        /// <response code="200">Returns the devices</response>
        /// <response code="400">If the request is invalid</response>
        [HttpPost("search")]
        [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DeviceResponse>>> GetDevices([FromBody] GetDevicesFilterRequest request, CancellationToken ct = default)
        {
            var query = request.ToFilterQuery();
            var result = await _dispatcher.QueryAsync<IEnumerable<DeviceDto>>(query, ct);

            var response = result.Select(DeviceResponse.FromDeviceDto);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new device
        /// </summary>
        /// <param name="request">Device creation details</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The newly created device</returns>
        /// <response code="200">Returns the newly created device</response>
        /// <response code="400">If the request is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeviceResponse>> CreateDevice([FromBody] CreateDeviceRequest request, CancellationToken ct = default)
        {
            var command = new CreateDeviceCommand(request.Name, request.Brand, request.State);
            var result = await _dispatcher.SendAsync<CreateDeviceCommand, DeviceDto>(command, ct);
            return Ok(DeviceResponse.FromDeviceDto(result));
        }
        
        /// <summary>
        /// Updates a device
        /// </summary>
        /// <param name="id">Existing device id to update</param>
        /// <param name="request">Device update details</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The updated device</returns>
        /// <response code="200">Returns the updated device</response>
        /// <response code="404">If the device was not found</response>
        /// <response code="400">If the request is invalid</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeviceResponse>> UpdateDevice(int id, [FromBody] PartialUpdateDeviceRequest request, CancellationToken ct = default)
        {
            var command = new UpdateDeviceCommand(id, request.Name, request.Brand, request.State);
            var result = await _dispatcher.SendAsync<UpdateDeviceCommand, DeviceDto>(command, ct);
            return Ok(DeviceResponse.FromDeviceDto(result));
        }

        /// <summary>
        /// Deletes a device
        /// </summary>
        /// <param name="id">Device id to delete</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Nothing</returns>
        /// <response code="204">Device was deleted</response>
        /// <response code="400">If the request is invalid</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDevice(int id, CancellationToken ct = default)
        {
            var command = new DeleteDeviceCommand(id);
            await _dispatcher.SendAsync<DeleteDeviceCommand>(command, ct);
            return NoContent();
        }
    }
}
