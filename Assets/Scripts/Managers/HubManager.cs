using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum Activity
{
    TavernTank = 0,
    TavernDPS = 1,
    TavernHeal = 2,
    TavernEarly = 3,
}

public class HubManager : MonoBehaviour
{
    public static Activity chosenActivity;
    public static HubManager instance;
    public static List<HeroType> remainingVagabonds = new();

    public List<Activity> availableActivities;
    public List<Sprite> activitySprites;

    public List<ActivityButton> activityButtons;
    

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        remainingVagabonds.Clear();
        foreach (var hero in RunManager.instance.heroes)
        {
            if (hero.definition.isVagabond)
            {
                if (!remainingVagabonds.Contains(hero.definition.type))
                {
                    remainingVagabonds.Add(hero.definition.type);
                }
            }
        }
        
        if(remainingVagabonds.Count > 0) //early taverns
        {
            ChooseActivity(Activity.TavernEarly);
        }
        else //other activities
        {
            var activitiesRolled = RollActivities();
            for (int i = 0; i < activityButtons.Count;i++)
            {
                activityButtons[i].Init(activitiesRolled[i]);
            }
        }
    }

    public List<Activity> RollActivities()
    {
        var activities = new List<Activity>();
        do
        {
            var selectedActivity = availableActivities[Random.Range(0, availableActivities.Count)];
            if (!activities.Contains(selectedActivity)) 
                activities.Add(selectedActivity);
        } while (activities.Count < activityButtons.Count );

        return activities;
    }

    public static Sprite GetSpriteForActivity(Activity linkedActivity)
    {
        return instance.activitySprites[(int)linkedActivity];
    }

    public static String GetActivityName(Activity linkedActivity)
    {
        switch(linkedActivity)
        {
            case Activity.TavernTank:
                return "Recruit Tank";
            case Activity.TavernDPS:
                return "Recruit DPS";
            case Activity.TavernHeal:
                return "Recruit Healer";
            default:
                throw new ArgumentOutOfRangeException(nameof(linkedActivity), linkedActivity, null);
        }
    }

    public void ChooseActivity(Activity activity)
    {
        chosenActivity = activity;
        switch (activity)
        {
            case Activity.TavernTank:
                SceneManager.LoadScene("TavernScene", LoadSceneMode.Additive);
                break;
            case Activity.TavernDPS:
                SceneManager.LoadScene("TavernScene", LoadSceneMode.Additive);
                break;
            case Activity.TavernHeal:
                SceneManager.LoadScene("TavernScene", LoadSceneMode.Additive);
                break;
            case Activity.TavernEarly:
                SceneManager.LoadScene("TavernScene", LoadSceneMode.Additive);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(activity), activity, null);
        }

        SceneManager.UnloadSceneAsync("HubScene");
    }
}
