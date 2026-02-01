using API.DTOs.Requests;
using Application.Devices.Queries.GetDevicesByFilter;
using Domain.Entities;
using System.Linq.Expressions;

namespace API.Extensions;

public static class FilterRequestExtensions
{

    //maps GetDevicesFilterRequest (API) to GetDevicesByFilterQuery (Application)
    public static GetDevicesByFilterQuery ToFilterQuery(this GetDevicesFilterRequest request)
    {
        Expression<Func<DeviceEntity, bool>> predicate = d => true;

        if (!string.IsNullOrEmpty(request.Brand))
        {
            var brandLower = request.Brand.ToLower();
            predicate = CombinePredicate(predicate, d => d.Brand.ToLower().Contains(brandLower));
        }

        if (request.State.HasValue)
        {
            var state = request.State.Value;
            predicate = CombinePredicate(predicate, d => d.State == state);
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            var nameLower = request.Name.ToLower();
            predicate = CombinePredicate(predicate, d => d.Name.ToLower().Contains(nameLower));
        }

        return new GetDevicesByFilterQuery(predicate);
    }

    private static Expression<Func<T, bool>> CombinePredicate<T>(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter)
        );
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}