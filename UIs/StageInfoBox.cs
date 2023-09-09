using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class StageInfoBox : MonoBehaviour
{

    [SerializeField] Text _txtStageNum;
    [SerializeField] Text _txtTitleName;
    [SerializeField] Text _txtPairCount;
    [SerializeField] Text _txtMonListCount;

    int _stageNum = 0;

    public void OpenStageInfoBox(DefineUtillHelper.stBattleStageInfo info)
    {
        gameObject.SetActive(true);
        _stageNum = info._stageNumber;
        _txtStageNum.text = string.Format("NO.<color=#FF0000>{0}</color>", info._stageNumber.ToString());
        _txtTitleName.text = info._name;
        _txtPairCount.text = info._pairCount.ToString();
        _txtMonListCount.text = info._monstersIndex.Length.ToString();
    }

    public void CloseBox()
    {
        gameObject.SetActive(false);
    }

    public void ClickGameStartButton()
    {
        //Debug.Log("게임 시작!!!");
        //SceneManager.LoadScene("IngameScene");
        SoundManager._instance.PlaySFXSound(DefineUtillHelper.eSFXClipKind.Button_sel);
        GameSceneManager._instance.StartIngameScene();
    }

    public void ClickCancelButton()
    {
        SoundManager._instance.PlaySFXSound(DefineUtillHelper.eSFXClipKind.Button_sel);
        CloseBox();
    }
}
