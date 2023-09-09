using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByObject : MonoBehaviour
{
    [SerializeField] Transform _goal;
    [SerializeField] GameObject _sub1;

    void Start()
    {
        iTween.MoveBy(gameObject, iTween.Hash("x", 5, "y", 3, "z", 20, "delay", 3, "time", 3, "easetype", iTween.EaseType.easeInOutBack, "oncomplete", "SubObjectMoveTo", "oncompletetarget", gameObject));
    }

    void SubObjectMoveTo()
    {
        iTween.MoveTo(_sub1, iTween.Hash("position", _goal.position, "time", 2, "delay", 1, "easetype", iTween.EaseType.easeInOutElastic));
    }
}
