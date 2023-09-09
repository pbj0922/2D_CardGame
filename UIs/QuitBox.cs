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
        Debug.Log("게임이 종료되었습니다.");
        Application.Quit();                                     // 플렛폼에서의 종료
        //UnityEditor.EditorApplication.isPlaying = false;        // 에디터에서의 종료
    }

    public void ClickNoButton()
    {
        gameObject.SetActive(false);
    }
}
