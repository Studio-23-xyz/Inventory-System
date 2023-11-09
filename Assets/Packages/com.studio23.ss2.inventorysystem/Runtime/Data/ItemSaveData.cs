using System;
using System.Collections;
using UnityEngine;

namespace Studio23.SS2.InventorySystem.Data
{
    /// <summary>
    /// Stores data per Itembase
    /// </summary>
    [Serializable]
    public class ItemSaveData
    {
        // to load an item saved data,
        // we first need to load the correctly named SO asset
        // from resources
        // hence, we need the name
        // We should not rely on the ItemBase class implementation to save that.
        // so this saves them as separate fields

        /// <summary>
        /// the item SO's asset name
        /// </summary>
        public string SOName;
        /// <summary>
        /// The saved data for an Item's internal state
        /// </summary>
        public string ItemData;

        public ItemSaveData(string SOName, string ItemData)
        {
            this.SOName = SOName;
            this.ItemData = ItemData;
        }
    }
}