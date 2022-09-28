using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System;
[Serializable]
public class EventController : SerializedMonoBehaviour
{
    [GUIColor(0, 1, 1, 1)]//blue


    public static EventController ins;
    [GUIColor(1, 1, 0, 1)]//yellow

    public List<Action> playerDieActions;

    private void Awake()
    {
        ins = this;
    }


    [GUIColor(0, 1, 0, 1)]//green

    [Button]
    public void CallEvent()
    {
    }


    [Button]
    public void AddEvent()
    {

    }
    [Button]
    public void AddEvent2()
    {

    }

    public void OnPlayerDieActions()
    {
        //playerDieActions.ForEach(action => {
        //    action.Invoke();
        //});
    }
  

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}

