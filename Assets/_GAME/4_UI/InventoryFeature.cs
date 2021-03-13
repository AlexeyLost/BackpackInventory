using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Doozy.Engine.UI;
using GAME.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

namespace GAME.UI.Inventory
{
    public class InventoryFeature : MonoBehaviour
    {
        private ItemsFeature _itemsFeature;
        
        public UIView inventoryPanel;
        public InventorySlot inventorSlotPrefab;
        public List<Sprite> inventorySlotSprites;

        private bool started;
        private List<InventorySlot> slots;
        private InventorySlot selectedSlot;

        private void Awake()
        {
            _itemsFeature = FindObjectOfType<ItemsFeature>();
        }

        private void OnEnable()
        {
            _itemsFeature.ShowInventory += ShowInventory;
        }

        private void OnDisable()
        {
            _itemsFeature.ShowInventory -= ShowInventory;
        }

        private void Start()
        {
            slots = new List<InventorySlot>();
        }

        private void ShowInventory()
        {
            //show inventory ui
            inventoryPanel.Show();
            
            //instantiate and init slots
            foreach (var item in _itemsFeature.itemsInBackPack)
            {
                InventorySlot slot = Instantiate(inventorSlotPrefab, inventoryPanel.transform);
                slot.image.sprite = inventorySlotSprites[item.id];
                slot.id = item.id;
                slots.Add(slot);
                started = true;
            }
        }

        private void Update()
        {
            if (started)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    //close inventory ui
                    inventoryPanel.Hide();
                    
                    //destroy slots
                    foreach (var slot in slots)
                    {
                        if (slot.selected)
                        {
                            selectedSlot = slot;
                        }
                    }
                    
                    //if some slot was selected take it from backpack
                    if (selectedSlot != null)
                    {
                        _itemsFeature.TakeFromBackpack?.
                        Invoke(_itemsFeature.itemsInBackPack.Find(it => it.id == selectedSlot.id));
                        selectedSlot = null;
                    }
                    //if no slot was selected allow drag and drop again
                    else
                    {
                        _itemsFeature.RestartDragDrop?.Invoke();
                    }

                    foreach (var slot in slots)
                    {
                        Destroy(slot.gameObject);
                    }
                    
                    slots.Clear();
                    started = false;
                }
            }
        }
    }
}
