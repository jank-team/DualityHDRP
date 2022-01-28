using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class DropPool<T> where T : Item
{
    private const int DefaultSize = 10;
    private readonly List<T> _items;
    private readonly Dictionary<Rank, List<T>> _rankedItems;

    private readonly Dictionary<Rank, int> _ranks = new Dictionary<Rank, int>
    {
        { Rank.Common, 100 },
        { Rank.Rare, 50 }
    };

    public DropPool(List<T> items, int size = DefaultSize)
    {
        _items = items;
        _rankedItems = items.GroupBy(item => item.Rank).ToDictionary(group => group.Key, group => group.ToList());
        UpdateCurrentItems(size);
    }

    public List<T> CurrentItems { get; private set; } = new List<T>();

    public void UpdateCurrentItems(int size = DefaultSize)
    {
        CurrentItems = _items.Count > 0
            ? Enumerable.Range(1, size).Select(i => GetRandomItem()).Where(item => item != null).ToList()
            : new List<T>();
    }


    [CanBeNull]
    private T GetRandomItem()
    {
        var value = Random.Range(0, 101);

        List<T> items = null;

        foreach (var kvp in _ranks)
        {
            if (value <= kvp.Value && _rankedItems.ContainsKey(kvp.Key))
            {
                items = _rankedItems[kvp.Key];
            }
            else
            {
                break;
            }
        }

        if (items == null) return null;

        var index = Random.Range(0, items.Count);

        return items[index];
    }
}
