using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootedItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public ItemDefinition item;
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
                Vector3 newPos = Input.GetTouch(0).position;
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

    public void Init()
    {
        itemPreview.sprite = item.itemImage;
        itemBackground.color = ItemTooltipManager.instance.rarityMap[item.rarity];
    }
    
    #region DragHandler

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        model.transform.localPosition = _modelInitialPos;

        Hero target = PlayerController.GetTarget<Hero>(eventData);

        if (target == null)
            return;
        
        LootManager.Instance.Choose(this, target);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LootManager.Instance.SelectItem(this);
    }

    #endregion
}