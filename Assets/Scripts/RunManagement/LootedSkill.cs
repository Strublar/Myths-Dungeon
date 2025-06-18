using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LootedSkill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public SkillDefinition definition;
    public Image skillPreview;
    public Image selectionFrame;
    public Image skillBackground;
    
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
        skillPreview.sprite = definition.skillImage;
        skillBackground.color = ItemTooltipManager.instance.rarityMap[definition.rarity];
    }
    
    #region Input Handler
    
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
        
        SkillLootManager.Instance.Choose(this, target);    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SkillLootManager.Instance.SelectSkill(this);
    }

    #endregion
}