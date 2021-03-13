using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAME.Items
{
    [CreateAssetMenu(fileName = "ItemSettings", menuName = "GAME/Settings/Item Settings")]
    public class ItemSettings : ScriptableObject
    {
        public float dragAndDropZPosition;
    }
}
