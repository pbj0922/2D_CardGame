using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultWindow : MonoBehaviour
{
    [SerializeField] Text _txtTitle;
    [SerializeField] Text _txtO_Count;
    [SerializeField] Text _txtX_Count;
    [SerializeField] GameObject _winRoot;
    [SerializeField] GameObject _loseRoot;

    AudioSource _sfx;

    float _countingTime = 1.5f;
    int _targetOCount = 0;
    int _targetXCount = 0;
    float _passOCount = 0;
    float _passXCount = 0;
    DefineUtillHelper.eCountingState _stateCount;

    // 임시
    //void Start()
    //{
    //    OpenWindow(true, 28, 16);    
    //}
    //===

    void LateUpdate()
    {
        switch(_stateCount)
        {
            case DefineUtillHelper.eCountingState.O_Count:
                _passOCount += _targetOCount * (Time.deltaTime / _countingTime);
                if(_passOCount >= _targetOCount)
                {
                    _passOCount = _targetOCount;
                    _stateCount = DefineUtillHelper.eCountingState.X_Count;
                }
                _txtO_Count.text = ((int)_passOCount).ToString();
                break;
            case DefineUtillHelper.eCountingState.X_Count:
                _passXCount += _targetXCount * (Time.deltaTime / _countingTime);
                if (_passXCount >= _targetXCount)
                {
                    _passXCount = _targetXCount;
                    _stateCount = DefineUtillHelper.eCountingState.CountingEnd;
                }
                _txtX_Count.text = ((int)_passXCount).ToString();
                break;
            case DefineUtillHelper.eCountingState.CountingEnd:
                break;
        }
    }

    public void OpenWindow(bool isWin, int oCount, int xCount)
    {
        if(isWin)
        {
            _winRoot.SetActive(true);
            _loseRoot.SetActive(false);
            _txtTitle.text = "WIN";
        }
        else
        {
            _winRoot.SetActive(false);
            _loseRoot.SetActive(true);
            _txtTitle.text = "LOSE";
        }
        _stateCount = DefineUtillHelper.eCountingState.O_Count;
        _passXCount = _passOCount = 0;
        _targetOCount = oCount;
        _targetXCount = xCount;
        _txtO_Count.text = ((int)_passOCount).ToString();
        _txtX_Count.text = ((int)_passXCount).ToString();

        _sfx = SoundManager._instance.PlaySFXSound(DefineUtillHelper.eSFXClipKind.Counting, true);
    }

    public void ClickHomeButton()
    {
        GameSceneManager._instance.StartLobbyScene();
    }
    public void ClickRegameButton()
    {
        // 임시
        //SceneManager.LoadScene("IngameScene");
        //===
        GameSceneManager._instance.StartIngameScene();
    }
    public void ClickNextButton()
    {
        UserInfoManager._instance._clearStageNumber = UserInfoManager._instance._nowStageToProceed++;
    }
}
