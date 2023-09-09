using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    static LobbyManager _uniqueInstance;

    //[SerializeField] GameObject _prefabStageSlot;

    Transform _rootPosition;
    Transform _rootSlot;
    StageInfoBox _stageInfoBox;
    QuitBox _quitBox;

    List<StageSlot> _stageList = new List<StageSlot>();

    int _clearStage = 0;

    public static LobbyManager _instance
    {
        get { return _uniqueInstance; }
    }

    void Awake()
    {
        _uniqueInstance = this;
    }

    void Start()
    {
        // юс╫ц
        //InitializeSetData(UserInfoManager._instance._clearStageNumber);
        //===
    }


    public void InitializeSetData(int clearNum)
    {
        _clearStage = clearNum;
        GameObject go = GameObject.FindGameObjectWithTag("SlotPositionRoot");
        _rootPosition = go.transform;
        go = GameObject.FindGameObjectWithTag("SlotRoot");
        _rootSlot = go.transform;
        go = GameObject.FindGameObjectWithTag("UIStageInfoBox");
        _stageInfoBox = go.GetComponent<StageInfoBox>();
        go = GameObject.FindGameObjectWithTag("UIQuitBox");
        _quitBox = go.GetComponent<QuitBox>();

        _stageInfoBox.CloseBox();
        _quitBox.ClickNoButton();

        for (int n = 0; n < _rootPosition.childCount; n++)
        {
            GameObject prefab = GameResourcePoolManager._instance.GetPrefabFromKey(DefineUtillHelper.ePrefabType.UI, (int)DefineUtillHelper.ePrefabUIs.StageSlot);
            go = Instantiate(prefab, _rootPosition.GetChild(n).position, Quaternion.identity, _rootSlot);
            StageSlot slot = go.GetComponent<StageSlot>();
            slot.InitData(n + 1, _clearStage);
            _stageList.Add(slot);
        }
    }

    public void StageSelect(int no)
    {
        for(int n = 0; n < _stageList.Count; n++)
        {
            _stageList[n].FreeSelected(no);
        }
        DefineUtillHelper.stBattleStageInfo info = GameResourcePoolManager._instance.GetStageInfoFrom(no);
        _stageInfoBox.OpenStageInfoBox(info);
        SoundManager._instance.PlaySFXSound(DefineUtillHelper.eSFXClipKind.Button_nor);
    }

    public void ClicKCharInfoButton()
    {
    }

    public void ClickSettingButton()
    {
    }

    public void ClickShopButton()
    {
    }

    public void ClickQuitButton()
    {
        _quitBox.OpenQuitBox();
    }
}
