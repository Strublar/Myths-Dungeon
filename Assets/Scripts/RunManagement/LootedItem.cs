using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootedItem : MonoBehaviour
{
    public int id;
    public Item item;
    public SpriteRenderer itemPreview;
    public SpriteRenderer rarityFrame;
    public SpriteRenderer itemBackground;
    public GameObject model;
    private bool isDragging;
    private Vector3 modelInitialPos;

    public void Start()
    {
        modelInitialPos = model.transform.localPosition;
    }
    public void Update()
    {
        if (Input.touchCount > 0)
        {
            if (isDragging)
            {
                Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                newPos.z = 0;
                model.transform.position = newPos;
            }
        }
        else
        {
            isDragging = false;
            model.transform.localPosition = modelInitialPos;
        }

    }
    public void OnStartDragging()
    {
        isDragging = true;
    }

    public void Init()
    {
        itemPreview.sprite = item.definition.itemImage;
        rarityFrame.color = ItemTooltipManager.instance.rarityMap[item.definition.rarity];
        itemBackground.color = Color.grey;
    }
    public void OnDrag(GameObject target)
    {
        if (target.CompareTag("Hero"))
        {
            
            Hero heroTarget = target.GetComponent<Hero>();

            heroTarget.item.definition = item.definition;
            heroTarget.item.qualityLevel = item.qualityLevel;
            heroTarget.LoadDefinition();

            LootManager.instance.Choose(id);
        }
        isDragging = false;
    }

    public void OnStayedHovered()
    {
        ItemTooltipManager.instance.InitItemTooltip(item);
        ItemTooltipManager.instance.ShowToolTip();
    }

    public void OnTap()
    {
        ItemTooltipManager.instance.InitItemTooltip(item);
        ItemTooltipManager.instance.ShowToolTip();
    }
}
