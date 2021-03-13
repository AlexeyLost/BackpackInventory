using System;
using System.Collections;
using System.Collections.Generic;
using GAME.Items;
using UnityEngine;

namespace GAME.Backpack
{
    public class BackpackRef : MonoBehaviour
    {
        public Transform bottlePosition;
        public Transform flashlightPosition;
        public Transform knifePosition;
        public List<Item> itemsInside;
    }
}
