using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GAME.UI.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int id;
        public Image image;
        public GameObject outline;
        public bool selected;

        //activate outline when mouse enter
        public void OnPointerEnter(PointerEventData eventData)
        {
            outline.SetActive(true);
            selected = true;
        }


        //remove outline on mouse exit
        public void OnPointerExit(PointerEventData eventData)
        {
            outline.SetActive(false);
            selected = false;
        }
    }
}
