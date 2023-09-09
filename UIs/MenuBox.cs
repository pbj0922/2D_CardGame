using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBox : MonoBehaviour
{
    public void OpenBox()
    {
        gameObject.SetActive(true);
    }

    public void CloseBox()
    {
        gameObject.SetActive(false);
    }
}
