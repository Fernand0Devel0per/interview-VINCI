using BuildingBlocks.Core.Enums;

namespace BuildingBlocks.Core.Events;

public class EntityChangedEvent<T>
{
    public string EntityType { get; set; }
    public EntityChangeType ChangeType { get; set; }
    public T Data { get; set; }

    public EntityChangedEvent(EntityChangeType changeType, T data)
    {
        EntityType = typeof(T).Name;
        ChangeType = changeType;
        Data = data;
    }
}