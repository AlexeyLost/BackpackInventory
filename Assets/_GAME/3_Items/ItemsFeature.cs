using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GAME.Backpack;
using GAME.Level;
using UnityEngine;

namespace GAME.Items
{
    public class ItemsFeature : MonoBehaviour
    {
        public Action ItemsInitialized;
        public Action<Item> PutInBackpack;
        public Action<Item> TakeFromBackpack;
        public Action ShowInventory;
        public Action RestartDragDrop;

        private LevelFeature _levelFeature;
        private BackpackFeature _backpackFeature;

        public ItemSettings settings;
        public List<Item> itemPrefabs;
        [HideInInspector] public List<Item> itemsInBackPack;
        [HideInInspector] public List<Item> items;

        private void Awake()
        {
            _levelFeature = FindObjectOfType<LevelFeature>();
            _backpackFeature = FindObjectOfType<BackpackFeature>();
        }

        private void OnEnable()
        {
            _levelFeature.LevelInitialized += InitItems;
            TakeFromBackpack += RemoveItemFromBackPack;
        }

        private void OnDisable()
        {
            _levelFeature.LevelInitialized -= InitItems;
            TakeFromBackpack -= RemoveItemFromBackPack;
        }

        private void Start()
        {
            itemsInBackPack = new List<Item>();
        }

        private void InitItems()
        {
            items = new List<Item>();
            foreach (var currentPrefab in itemPrefabs)
            {
                //spawn item
                Item _item = Instantiate(currentPrefab);
                
                //check if item in backpack
                if (PlayerPrefs.HasKey(_item.title)) 
                    _item.inBackpack = PlayerPrefs.GetInt(_item.title) == 1;
                else 
                    _item.inBackpack = false;
                
                
                //init item Backpack position
                switch (_item.title)
                {
                    case "Flashlight":
                        _item.backPackPosition = _backpackFeature.backpack.flashlightPosition;
                        break;
                    case "Bottle":
                        _item.backPackPosition = _backpackFeature.backpack.bottlePosition;
                        break;
                    case "Knife":
                        _item.backPackPosition = _backpackFeature.backpack.knifePosition;
                        break;
                }
                if (_item.inBackpack)
                {
                    _item.body.isKinematic = true;
                    _item.transform.position = _item.backPackPosition.position;
                    _item.transform.rotation = _item.backPackPosition.rotation;
                    _item.gameObject.layer = 0;
                    itemsInBackPack.Add(_item);
                }

                //store item in list
                items.Add(_item);
            }
            
            ItemsInitialized?.Invoke();
        }

        private void RemoveItemFromBackPack(Item item)
        {
            item.gameObject.layer = 8;
            item.inBackpack = false;
            itemsInBackPack.Remove(item);
            SaveData();
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void SaveData()
        {
            foreach (var item in items)
            {
                PlayerPrefs.SetInt(item.title, item.inBackpack ? 1 : 0);
            }
        }
    }
}
