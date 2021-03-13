using System;
using System.Collections;
using System.Collections.Generic;
using GAME.Items;
using UnityEngine;
using UnityEngine.Networking;

namespace GAME.Web
{
    public class WebFeature : MonoBehaviour
    {
        private ItemsFeature _itemsFeature;

        private void Awake()
        {
            _itemsFeature = FindObjectOfType<ItemsFeature>();
        }

        private void OnEnable()
        {
            _itemsFeature.PutInBackpack += PutPostQuery;
            _itemsFeature.TakeFromBackpack += TakePostQuery;
        }

        private void OnDisable()
        {
            _itemsFeature.PutInBackpack -= PutPostQuery;
            _itemsFeature.TakeFromBackpack -= TakePostQuery;
        }

        private void PutPostQuery(Item item)
        {
            StartCoroutine(POST(item, "put"));
        }

        private void TakePostQuery(Item item)
        {
            StartCoroutine(POST(item, "take"));
        }
        
        private IEnumerator POST(Item item, string eventType)
        {
            WWWForm form = new WWWForm();
            form.AddField("auth", "BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6");
            form.AddField("id", item.id.ToString());
            form.AddField("event", eventType);
            
            using (UnityWebRequest www = UnityWebRequest.Post("https://dev3r02.elysium.today/inventory/status", form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Post Request Complete!");
                }
            }
        }
    }
}
