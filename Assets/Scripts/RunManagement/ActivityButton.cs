using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivityButton : MonoBehaviour
{
    public Activity linkedActivity;
    public SpriteRenderer activityImage;
    public TextMeshPro activityName;

    public void Init(Activity activity)
    {
        linkedActivity = activity;
        activityImage.sprite = HubManager.GetSpriteForActivity(linkedActivity);
        activityName.text = HubManager.GetActivityName(linkedActivity);
    }

    public void OnTap()
    {
        HubManager.instance.ChooseActivity(linkedActivity);
    }
}
