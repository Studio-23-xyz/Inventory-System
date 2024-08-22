using UnityEngine;
using UnityEngine.Localization;

namespace Studio23.SS2.InventorySystem.Data
{
    public abstract class ItemBase : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Description;

        public LocalizedString NameLocalizedString;
        public LocalizedString DescriptionLocalizedString;
        public abstract void AssignSerializedData(string data);
        public abstract string GetSerializedData();
    }
}
