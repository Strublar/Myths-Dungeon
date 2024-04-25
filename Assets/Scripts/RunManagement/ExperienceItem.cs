using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceItem : MonoBehaviour
{
    public int id;
    private bool isDragging;
    private Vector3 modelInitialPos;
    public GameObject model;
    public void OnDrag(GameObject target)
    {
        if (target.CompareTag("Hero"))
        {
            Hero heroTarget = target.GetComponent<Hero>();
            heroTarget.LoadDefinition();
            LootManager.instance.Choose(id);
        }
        isDragging = false;
    }

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
}
