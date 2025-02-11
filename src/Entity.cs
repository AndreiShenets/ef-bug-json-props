namespace EfCoreException;

public class Entity
{
    public required Guid Id { get; set; }

    public required List<OwnedEntityLevel1> L1OwnedEntities { get; set; }
}

public sealed class OwnedEntityLevel1
{
    public required List<OwnedEntityLevel2> L2OwnedEntities { get; set; }
}

public sealed class OwnedEntityLevel2
{
    public required List<EntityValue> Values { get; init; }
}

public sealed class EntityValue
{
    public required string Name { get; init; }
    public required string Value { get; init; }
}