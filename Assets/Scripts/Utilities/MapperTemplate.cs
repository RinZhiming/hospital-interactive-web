using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Mapper<TKey, TValue>
{
    [SerializeField] private TKey key;
    [SerializeField] private TValue value;
    
    public TKey Key
    {
        get => key;
        set => key = value;
    }

    public TValue Value
    {
        get => value;
        set => this.value = value;
    }

    
}

[Serializable]
public class MapperGroup<TKey, TValue>
{
    [SerializeField] private List<Mapper<TKey, TValue>> mappers;

    public List<Mapper<TKey, TValue>> Mappers
    {
        get => mappers;
        set => mappers = value;
    }

}