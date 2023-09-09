using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitBox : MonoBehaviour
{
    public void OpenQuitBox()
    {
        gameObject.SetActive(true);
    }

    public void ClickYesButton()
    {
        Debug.Log("������ ����Ǿ����ϴ�.");
        Application.Quit();                                     // �÷��������� ����
        //UnityEditor.EditorApplication.isPlaying = false;        // �����Ϳ����� ����
    }

    public void ClickNoButton()
    {
        gameObject.SetActive(false);
    }
}
