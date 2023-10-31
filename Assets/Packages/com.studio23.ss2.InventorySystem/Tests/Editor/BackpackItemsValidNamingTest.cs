using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NUnit.Framework;
using Studio23.SS2.InventorySystem.Data;

public class BackpackItemsValidNamingTest
{
    private static IEnumerable<Item> _allItems => Resources.LoadAll<Item>("Inventory System/Backpack").ToList();

    [TestCaseSource(nameof(_allItems))]
    public void ValidNamingCase(Item item)
    {
        Assert.AreEqual(item.name, item.Name);
    }
}
