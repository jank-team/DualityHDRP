using System.Collections.Generic;

public interface IBuilding<T>
{
    List<T> GetItems();
    void UpdateItems();
    void Upgrade();
}
