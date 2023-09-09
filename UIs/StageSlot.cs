using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSlot : MonoBehaviour
{
    [SerializeField] Image _lockIcon;
    [SerializeField] Text _txtClearRank;
    [SerializeField] Image _slotBG;
    // �ӽ�
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
            // ���� ����
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
            // ���� ���
            _nowState = DefineUtillHelper.eStageSlotStage.Lock;
        }

        if(_nowState >= DefineUtillHelper.eStageSlotStage.Free)
        {
            // ���� ������ BG �Ķ������� ����
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
            // ���� ���¿� ���� ��ġ ���°� ���ϵ��� �ؾ���
            _nowState = DefineUtillHelper.eStageSlotStage.Select;
            _slotBG.sprite = _bgImages[(int)_nowState];
            LobbyManager._instance.StageSelect(_stageNum);
        }
    }

    //void OnMouseDown()
    //{
    //    if (_nowState == DefineUtillHelper.eStageSlotStage.Free)
    //    {
    //        // ���� ���¿� ���� ��ġ ���°� ���ϵ��� �ؾ���
    //        _nowState = DefineUtillHelper.eStageSlotStage.Select;
    //        _slotBG.sprite = _bgImages[(int)_nowState];
    //        LobbyManager._instance.StageSelect(_stageNum);
    //        // �������� ����â ����
    //    }
    //}
}
