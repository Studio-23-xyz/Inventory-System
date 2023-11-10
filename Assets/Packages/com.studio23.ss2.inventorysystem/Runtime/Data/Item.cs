using System;
using UnityEngine;

namespace Studio23.SS2.InventorySystem.Data
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Studio-23/Inventory System/New Item")]
    [Serializable]
    public class Item : ItemBase
    {
        public Sprite Icon;
        public override void AssignSerializedData(string data)
        {
            
        }

        public override string GetSerializedData()
        {
            return "";
        }
    }

}
