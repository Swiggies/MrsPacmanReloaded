﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    //private Tween activeTween;
    private List<Tween> activeTweens = new List<Tween>();
    public static Tweener _Instance;     

    void Awake()
    {
        _Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < activeTweens.Count; i++)
        {
            if (Vector3.Distance(activeTweens[i].Target.position, activeTweens[i].EndPos) > 0.1f)
            {
                float timeFraction = (Time.time - activeTweens[i].StartTime) / activeTweens[i].Duration;
                //timeFraction = Mathf.Pow(timeFraction, 3);
                activeTweens[i].Target.position = Vector3.Lerp(activeTweens[i].StartPos, activeTweens[i].EndPos, timeFraction);
            }
            else
            {
                activeTweens[i].Target.position = activeTweens[i].EndPos;
                activeTweens.RemoveAt(i);
            }
        }
    }

    public bool TweenExists(Transform target)
    {
        for (int i = 0; i < activeTweens.Count; i++)
        {
            if (activeTweens[i].Target == target)
                return true;
        }
        return false;
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (!TweenExists(targetObject))
        {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
            return true;
        }
        return false;
    }

    public bool CancelTween(Transform targetObject)
    {
        if (TweenExists(targetObject))
        {
            for (int i = 0; i < activeTweens.Count; i++)
            {
                if (activeTweens[i].Target == targetObject)
                {
                    activeTweens.RemoveAt(i);
                    return true;
                }
            }
        }
        return false;
    }
}
