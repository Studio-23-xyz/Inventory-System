using System.Collections.Generic;
using System.Linq;
using com.studio23.ss2.inventorysystem.data;
using NUnit.Framework;
using UnityEngine;

public class CorrectNaming
{
    private static IEnumerable<Item> _allItems => Resources.LoadAll<Item>("Items").ToList();

    [TestCaseSource(nameof(_allItems))]
    public void CorrectNamingCase(Item item)
    {
        Assert.AreEqual(item.name, item.ItemName);
    }
}
