using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    [SerializeField] Text _txtMessage;

    public void OpenMessageBox(string contentTxt)
    {
        gameObject.SetActive(true);
        _txtMessage.text = contentTxt;
    }
    public void CloseBox()
    {
        gameObject.SetActive(false);
    }
}
