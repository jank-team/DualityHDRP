using System.Collections.Generic;

public class WeaponShop : Building<Weapon>
{
    protected override DropPool<Weapon> GetDropPool()
    {
        var items = new List<Weapon>
        {
            new Weapon
            {
                Attack = 1,
                Name = "Stick",
                Rank = Rank.Common,
                Value = 10
            },
            new Weapon
            {
                Attack = 2,
                Name = "Big Stick",
                Rank = Rank.Rare,
                Value = 20
            }
        };
        return new DropPool<Weapon>(items);
    }
}
