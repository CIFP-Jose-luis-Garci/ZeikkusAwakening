using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotManager : MonoBehaviour, ISelectHandler
{
    public Item item;
    public GameObject popup;
    public GameObject botonesBolsa, botonesItem;
    private AudioSource source;
    public AudioClip sonidoSeleccionar;

    private void Start()
    {
        source = GameManager.Instance.source;
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            source.PlayOneShot(sonidoSeleccionar);
            transform.parent.parent.gameObject.GetComponent<ListaItemsManager>().itemSelected = true;
            GameObject instantiatedPopup = Instantiate(popup, transform.position, transform.rotation, GameObject.FindGameObjectWithTag("UI").transform);
            RectTransform rectTransform = instantiatedPopup.GetComponent<RectTransform>();
            Vector2 position = rectTransform.anchoredPosition;
            position.x += 500;
            position.y -= 100;
            rectTransform.anchoredPosition = position;
            instantiatedPopup.GetComponentInChildren<Button>().Select();
            ItemPopupManager iPopManager = instantiatedPopup.GetComponent<ItemPopupManager>();
            iPopManager.SetItem(item);
            iPopManager.botonesBolsa = botonesBolsa;
            iPopManager.botonesItem = botonesItem;
            botonesBolsa.SetActive(false);
            botonesItem.SetActive(true);
        });
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.parent.parent.gameObject.GetComponent<ListaItemsManager>().itemSelected = false;
    }
}
