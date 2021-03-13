using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using GAME.Backpack;
using GAME.Level;
using GAME.UI.Inventory;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GAME.Items
{
    public class DragAndDropLogic : MonoBehaviour
    {
        private ItemsFeature _itemsFeature;

        private bool started;
        private Camera mainCam;
        private Ray ray;
        private RaycastHit hit;
        private LayerMask itemsMask;
        private LayerMask backpackMask;
        private Item itemInHand;
        private BackpackRef _backpack;

        private void Awake()
        {
            _itemsFeature = GetComponent<ItemsFeature>();
        }

        private void OnEnable()
        {
            _itemsFeature.ItemsInitialized += StartDragAndDrop;
            _itemsFeature.RestartDragDrop += RestartDragDrop;
            _itemsFeature.TakeFromBackpack += TakeItemAndRestartDragAndDrop;
        }

        private void OnDisable()
        {
            _itemsFeature.ItemsInitialized -= StartDragAndDrop;
            _itemsFeature.RestartDragDrop -= RestartDragDrop;
            _itemsFeature.TakeFromBackpack -= TakeItemAndRestartDragAndDrop;
        }

        private void StartDragAndDrop()
        {
            //and init variables
            mainCam = Camera.main;
            itemsMask = LayerMask.GetMask("Items");
            backpackMask = LayerMask.GetMask("Backpack");
            started = true;
        }
        
        private void RestartDragDrop()
        {
            started = true;
        }

        private void TakeItemAndRestartDragAndDrop(Item currentItem)
        {
            //smooth move item to hold point, then activate drag and drop
            currentItem.body.isKinematic = true;
            Vector3 point = Input.mousePosition;
            point.z = _itemsFeature.settings.dragAndDropZPosition;
            currentItem.body.DOMove(mainCam.ScreenToWorldPoint(point), 0.1f).OnComplete(() =>
            {
                itemInHand = currentItem;
                itemInHand.backPackPosition.gameObject.SetActive(true);
                RestartDragDrop();
            });
        }

        private void Update()
        {
            if (started)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ray = mainCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100f, itemsMask))
                    {
                        //if click on item take it
                        itemInHand = hit.collider.GetComponent<Item>();
                        itemInHand.body.isKinematic = true;
                        itemInHand.backPackPosition.gameObject.SetActive(true);
                    } else if (Physics.Raycast(ray, out hit, 100f, backpackMask))
                    {
                        //if click on backpack open inventory ui
                        if (_itemsFeature.itemsInBackPack.Count > 0)
                        {
                            started = false;
                            _itemsFeature.ShowInventory?.Invoke();
                        }
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (itemInHand != null)
                    {
                        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 100f, backpackMask))
                        {
                            //put item in backpack if mouse up over backpack
                            ItemToBackpack();
                        }
                        else
                        {
                            //release item if mouse up over table
                            itemInHand.body.isKinematic = false;
                            itemInHand.backPackPosition.gameObject.SetActive(false);
                            itemInHand = null;
                        }
                        
                    }
                }

                if (itemInHand != null)
                {
                    //if item in hand move it
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = _itemsFeature.settings.dragAndDropZPosition;
                    itemInHand.body.position = mainCam.ScreenToWorldPoint(mousePos);
                }
            }
        }

        private void ItemToBackpack()
        {
            itemInHand.inBackpack = true;
            _itemsFeature.itemsInBackPack.Add(itemInHand);
            itemInHand.gameObject.layer = 0;
            itemInHand.backPackPosition.gameObject.SetActive(false);
            itemInHand.body.DOMove(itemInHand.backPackPosition.position, 0.5f).SetEase(Ease.InExpo);
            itemInHand.body.DORotate(itemInHand.backPackPosition.eulerAngles, 0.4f);
            _itemsFeature.PutInBackpack?.Invoke(itemInHand);
            itemInHand = null;
        }
    }
}
