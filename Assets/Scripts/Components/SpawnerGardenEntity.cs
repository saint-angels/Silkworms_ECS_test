using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct SpawnerGardenEntity : IComponentData
{
    // Add fields to your component here. Remember that:
    //
    // * A component itself is for storing data and doesn't 'do' anything.
    //
    // * To act on the data, you will need a System.
    //
    // * Data in a component must be blittable, which means a component can
    //   only contain fields which are primitive types or other blittable
    //   structs; they cannot contain references to classes.
    //
    // * You should focus on the data structure that makes the most sense
    //   for runtime use here. Authoring Components will be used for 
    //   authoring the data in the Editor.

    public Entity prefabEarth;
    public Entity prefabWorm;
    public Entity prefabLeaf;

    public Entity GetEntityForType(EntityType type)
    {
        switch (type)
        {
            case EntityType.EARTH:
                return prefabEarth;
            case EntityType.WORM:
                return prefabWorm;
            case EntityType.FOOD:
                return prefabLeaf;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
