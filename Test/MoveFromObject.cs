using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFromObject : MonoBehaviour
{
    void Start()
    {
        iTween.MoveFrom(gameObject, iTween.Hash("y", 5, "z", 10, "time", 2, "easetype", iTween.EaseType.easeInOutElastic));
    }
}
