using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] GameObject _sub1;
    [SerializeField] GameObject _sub2;
    [SerializeField] Transform _goal;

    void Start()
    {
        iTween.RotateBy(gameObject, iTween.Hash("x", 30, "y", 30, "z", 20, "time", 20));
        iTween.MoveTo(_sub1, iTween.Hash("position", gameObject.transform.position, "time", 3, "delay", 1, "easetype", iTween.EaseType.linear/*, "oncomplete", "MoveCapsule", "oncompletetarget", _sub1*/));
        iTween.MoveTo(gameObject, iTween.Hash("position", _goal.position, "time", 1, "delay", 3.5, "easetype", iTween.EaseType.linear));
        iTween.RotateBy(_sub2, iTween.Hash("x", 10, "y", 10, "z", 5, "time", 10, "delay", 5));
        iTween.MoveTo(_sub2, iTween.Hash("position", gameObject.transform.position, "time", 5, "delay", 5, "easetype", iTween.EaseType.linear));
        iTween.ScaleTo(_sub2, iTween.Hash("x", 5, "y", 5, "z", 5, "time", 5, "delay", 5));
    }

    void MoveCapsule()
    {
        iTween.RotateBy(_sub2, iTween.Hash("x", 10, "y", 30, "z", 20, "time", 10, "delay", 5));
        iTween.MoveTo(_sub2, iTween.Hash("position", gameObject.transform.position, "time", 5, "delay", 5, "easetype", iTween.EaseType.linear));
        iTween.ScaleBy(_sub2, iTween.Hash("amount", 2, "time", 5, "delay", 5));
    }
}
