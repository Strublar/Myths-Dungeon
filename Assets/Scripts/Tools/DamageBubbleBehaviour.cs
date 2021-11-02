using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageBubbleBehaviour : MonoBehaviour
{
    public TextMeshPro text;
    public float speed;
    public float duration;

    private float baseDuration;
    private void Start()
    {
        baseDuration = duration;
    }
    // Update is called once per frame
    void Update()
    {
        text.transform.position += speed * Time.deltaTime * Vector3.up ;
        text.color = new Color(text.color.r, text.color.g, text.color.b, duration / baseDuration);
        duration -= Time.deltaTime;
        if(duration<0)
        {
            Destroy(text.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        Destroy(text.gameObject);
        Destroy(gameObject);
    }
}
