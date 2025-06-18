using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //General touch attributes
    private Touch touch;
    private RaycastHit2D raycast;
    private GameObject objectHit;

    //Dragging components
    private bool isDragging;
    private GameObject initialHit;

    //Hover components
    [SerializeField] private float hoverTimer;
    private float currentHoverTimer;
    // Update is called once per frame
    /*void Update()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            
            raycast = Physics2D.Raycast(touch.position, Camera.main.transform.forward);
            objectHit = raycast.collider ? raycast.collider.gameObject : null ;
            if(objectHit == null)
            {
                return;
            }

            //Detect initial touch
            if (touch.phase == TouchPhase.Began)
            {
                currentHoverTimer = hoverTimer;
                initialHit = objectHit;
            }




            //Detect start drag
            else if (touch.phase == TouchPhase.Moved && initialHit != null)
            {
                
                initialHit.SendMessage("OnStartDragging", SendMessageOptions.DontRequireReceiver);
            }

            //Detect tap
            else if (touch.phase == TouchPhase.Ended && initialHit == objectHit)
            {
                //Debug.Log("Tapped " + objectHit.name);
                objectHit.SendMessage("OnTap", SendMessageOptions.DontRequireReceiver);
            }

            //Detect drag
            else if (touch.phase == TouchPhase.Ended && initialHit != objectHit && initialHit != null)
            {
                //Debug.Log("Dragged from"+initialHit.name+ " to"  + objectHit.name);
                initialHit.SendMessage("OnDrag", objectHit, SendMessageOptions.DontRequireReceiver);
            }

            //Detect stay
            else if (touch.phase == TouchPhase.Stationary && objectHit == initialHit)
            {
                currentHoverTimer -= Time.deltaTime;
                if (currentHoverTimer < 0)
                {
                    objectHit.SendMessage("OnStayedHovered", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }*/

    public static T GetTarget<T>(PointerEventData eventData)
    {
        var raycastResults = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventData, raycastResults);

        T target = raycastResults.Select(r => r.gameObject.GetComponent<T>()).FirstOrDefault(resultEntity => resultEntity != null);
        
        return target;
    } 
}
