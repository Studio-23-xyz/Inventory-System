using Studio23.SS2.SaveSystem.Interfaces;
using System;
using UnityEngine;
namespace Studio23.SS2.InventorySystem.Data
{
    public class ItemBase : ScriptableObject, ISaveable
    {
        public string Id;
        public string Name;
        public string Description;


        // Just Id is probably not a unique path
        // We don't use IDs to save these to separate files
        // So this shouldn't matter
        public virtual string UniqueID => Id;

        public virtual void AssignSerializedData(string data)
        {
            // the itemBase doesn't need to save its fields
            // as it none of them are stateful values that can change.
            return;
        }

        public virtual string GetSerializedData()
        {
            // the itemBase doesn't need to load its fields
            // as it none of them are stateful values that can change.
            return "";
        }
    }
}
