using BuildingBlocks.Core.Enums;

namespace OrderService.QueryAPI.Application.Common.Abstractions;

public interface IEntityChangeHandler
{
    Task HandleAsync(string rawData, EntityChangeType changeType, CancellationToken cancellationToken);
}