using Application.CQRS;
using Application.Devices.DTOs;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Devices.Queries.GetDevicesByFilter;

public record GetDevicesByFilterQuery(
    Expression<Func<DeviceEntity, bool>> Predicate
) : IQuery<IEnumerable<DeviceDto>>;