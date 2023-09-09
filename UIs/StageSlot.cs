using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSlot : MonoBehaviour
{
    [SerializeField] Image _lockIcon;
    [SerializeField] Text _txtClearRank;
    [SerializeField] Image _slotBG;
    // 임시
    [SerializeField] Sprite[] _bgImages;
    //===

    int _stageNum;
    DefineUtillHelper.eStageSlotStage _nowState;

    //LineRenderer _line;

    void Awake()
    {
        //_line = GetComponent<LineRenderer>();
    }

    public void InitData(int no, int clearStageNum, int rank = 0)
    {
        _stageNum = no;
        if(rank > 0)
        {
            _txtClearRank.text = rank.ToString();
        }
        else
        {
            _txtClearRank.enabled = false;
        }

        if(clearStageNum >= no - 1)
        {
            // 열쇠 해지
            if (UserInfoManager._instance._nowStageToProceed == no)
            {
                _nowState = DefineUtillHelper.eStageSlotStage.Select;
            }
            else
            {
                _nowState = DefineUtillHelper.eStageSlotStage.Free;
            }
        }
        else
        {
            // 열쇠 잠금
            _nowState = DefineUtillHelper.eStageSlotStage.Lock;
        }

        if(_nowState >= DefineUtillHelper.eStageSlotStage.Free)
        {
            // 열쇠 빠지고 BG 파란색으로 변경
            _slotBG.sprite = _bgImages[(int)_nowState];
            _lockIcon.gameObject.SetActive(false);
        }
    }

    public void FreeSelected(int no)
    {
        if(_stageNum != no && _nowState != DefineUtillHelper.eStageSlotStage.Lock)
        {
            _nowState = DefineUtillHelper.eStageSlotStage.Free;
            _slotBG.sprite = _bgImages[(int)_nowState];
        }
    }

    public void ClickStageButton()
    {
        if (_nowState >= DefineUtillHelper.eStageSlotStage.Free)
        {
            // 나의 상태에 따라 터치 상태가 변하도록 해야함
            _nowState = DefineUtillHelper.eStageSlotStage.Select;
            _slotBG.sprite = _bgImages[(int)_nowState];
            LobbyManager._instance.StageSelect(_stageNum);
        }
    }

    //void OnMouseDown()
    //{
    //    if (_nowState == DefineUtillHelper.eStageSlotStage.Free)
    //    {
    //        // 나의 상태에 따라 터치 상태가 변하도록 해야함
    //        _nowState = DefineUtillHelper.eStageSlotStage.Select;
    //        _slotBG.sprite = _bgImages[(int)_nowState];
    //        LobbyManager._instance.StageSelect(_stageNum);
    //        // 스테이지 정보창 열림
    //    }
    //}
}
