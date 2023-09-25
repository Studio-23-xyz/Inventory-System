using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using com.studio23.ss2.inventorysystem.data;
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
