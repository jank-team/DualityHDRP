using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Building<T> : MonoBehaviour, IBuilding<T> where T : Item
{
    private DropPool<T> _dropPool;
    private int _level = 0;

    public void Awake()
    {
        _dropPool = GetDropPool();
    }

    protected abstract DropPool<T> GetDropPool();

    public List<T> GetItems()
    {
        return _dropPool.CurrentItems;
    }

    public void UpdateItems()
    {
        var dropPoolSize = 10 + _level;
        _dropPool.UpdateCurrentItems(dropPoolSize);
    }

    public void Upgrade()
    {
        _level += 1;
    }
}