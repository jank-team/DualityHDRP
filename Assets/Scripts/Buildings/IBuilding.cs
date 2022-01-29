using System.Collections.Generic;

public interface IBuilding
{
    List<Item> GetItems();
    void UpdateItems();
    void Upgrade();
}
