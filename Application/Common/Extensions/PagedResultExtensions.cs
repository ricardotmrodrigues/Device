using Application.Common.Models;
using Domain.Common;

namespace Application.Common.Extensions;

public static class PagedResultExtensions
{
    public static PaginatedList<TDestination> ToPaginatedList<TSource, TDestination>(
        this PagedResult<TSource> pagedResult,
        Func<TSource, TDestination> mapper)
    {
        var mappedItems = pagedResult.Items.Select(mapper).ToList();
        return new PaginatedList<TDestination>(mappedItems, pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize);
    }

    public static PaginatedList<T> ToPaginatedList<T>(this PagedResult<T> pagedResult)
    {
        return new PaginatedList<T>(pagedResult.Items.ToList(), pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize);
    }
}