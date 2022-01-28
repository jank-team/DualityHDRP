using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShop : Building<Weapon>
{
    protected override DropPool<Weapon> GetDropPool()
    {
        var items = new List<Weapon>()
        {
            new Weapon()
            {
                Attack = 1,
                Name = "Stick",
                Rank = Rank.Common,
                Value = 10
            }
        };
        return new DropPool<Weapon>(items);
    }
}
