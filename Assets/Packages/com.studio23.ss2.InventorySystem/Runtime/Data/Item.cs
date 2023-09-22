using System;
using UnityEngine;

namespace com.studio23.ss2.inventorysystem.data
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    [Serializable]
    public class Item : ScriptableObject
    {
        public string ItemName;
        public string Description;
        public Sprite Icon;
    }
}
