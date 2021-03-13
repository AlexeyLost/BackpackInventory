using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAME.Items
{
    [Serializable]
    public class Item : MonoBehaviour
    {
        public int id;
        public string title;
        public float weight;
        public Rigidbody body;
        public ItemType _type;
        [HideInInspector] public Transform backPackPosition;
        public bool inBackpack;
    }
}
