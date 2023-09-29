using System;
using System.Collections;
using System.Collections.Generic;
using com.studio23.ss2.inventorysystem.data;
using UnityEngine;

namespace com.studio23.ss2.inventorysystem.data
{
    [CreateAssetMenu(fileName = "New Blueprint", menuName = "Blueprint/New")]
    [Serializable]
    public class Blueprint : ScriptableObject
    {
        public List<ItemBase> Components;
        public ItemBase Product;
    }
}