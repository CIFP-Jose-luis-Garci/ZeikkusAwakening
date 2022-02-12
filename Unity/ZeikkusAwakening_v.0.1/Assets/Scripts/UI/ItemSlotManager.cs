using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotManager : MonoBehaviour
{
    public Item item;
    public GameObject popup;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            transform.parent.parent.gameObject.GetComponent<ListaItemsManager>().itemSelected = true;
            GameObject instantiatedPopup = Instantiate(popup, transform.position, transform.rotation, FindObjectOfType<Canvas>().transform);
            RectTransform rectTransform = instantiatedPopup.GetComponent<RectTransform>();
            Vector2 position = rectTransform.anchoredPosition;
            position.x += 500;
            position.y -= 100;
            rectTransform.anchoredPosition = position;
            instantiatedPopup.GetComponentInChildren<Button>().Select();
            instantiatedPopup.GetComponent<ItemPopupManager>().SetItem(item);
        });
    }
}
