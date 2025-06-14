using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootedItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    public Image itemPreview;
    public Image selectionFrame;
    public Image itemBackground;
    public GameObject model;
    private bool _isDragging;
    private Vector3 _modelInitialPos;

    public void Start()
    {
        _modelInitialPos = model.transform.localPosition;
    }
    public void Update()
    {
        if (Input.touchCount > 0)
        {
            if (_isDragging)
            {
                Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                newPos.z = 0;
                model.transform.position = newPos;
            }
        }
        else
        {
            _isDragging = false;
            model.transform.localPosition = _modelInitialPos;
        }

    }
    public void OnStartDragging()
    {
        _isDragging = true;
    }

    public void Init()
    {
        itemPreview.sprite = item.definition.itemImage;
        itemBackground.color = ItemTooltipManager.instance.rarityMap[item.rarity];
    }
    public void OnDrag(GameObject target)
    {
        if (target.CompareTag("Hero"))
        {
            Hero heroTarget = target.GetComponent<Hero>();
            
            LootManager.instance.Choose(this, heroTarget);
        }
        _isDragging = false;
    }
    public void OnTap()
    {
        LootManager.instance.SelectItem(this);
    }
    

    #region DragHandler
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        model.transform.position = new Vector3(eventData.position.x, eventData.position.y, 0);
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        model.transform.localPosition = _modelInitialPos;
    }

    #endregion
}
