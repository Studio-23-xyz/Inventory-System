using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Studio23.SS2.InventorySystem.Data;
using NUnit.Framework;

public class BackpackItemsValidNamingTest
{
    private static IEnumerable<Item> _allItems => Resources.LoadAll<Item>("Backpack").ToList();

    [TestCaseSource(nameof(_allItems))]
    public void ValidNamingCase(Item item)
    {
        Assert.AreEqual(item.name, item.Name);
    }
}
