using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    static GameSceneManager _uniqueInstance;

    DefineUtillHelper.eSceneNumber _nowScene;

    AsyncOperation _asyncOper;
    LoaddingWindow _wndLoadding;

    // 임시
    float _delayTime = 0;
    //===

    public static GameSceneManager _instance
    {
        get { return _uniqueInstance; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //Debug.Log(_asyncOper.progress);    
        if(_asyncOper != null)
        {
            if(_asyncOper.isDone)
            {
                _delayTime += Time.deltaTime;
                if (_delayTime >= 5.5f)
                {
                    if (_wndLoadding != null)
                    {
                        _wndLoadding.CloseWnd();
                    }
                    _asyncOper = null;

                    switch(_nowScene)
                    {
                        case DefineUtillHelper.eSceneNumber.Lobby:
                            LobbyManager._instance.InitializeSetData(UserInfoManager._instance._clearStageNumber);
                            break;
                        case DefineUtillHelper.eSceneNumber.Ingame:
                            DefineUtillHelper.stBattleStageInfo stageInfo = GameResourcePoolManager._instance.GetStageInfoFrom(UserInfoManager._instance._nowStageToProceed);
                            IngameManger._instance.InitalizeSetting(stageInfo);
                            break;
                    }
                }
            }
        }
    }

    void Start()
    {
        // 임시
        StartLobbyScene(true);
        //===
    }

    public void StartLobbyScene(bool isFirst = false)
    {
        _asyncOper = SceneManager.LoadSceneAsync("LobbyScene");
        _nowScene = DefineUtillHelper.eSceneNumber.Lobby;
        _delayTime = 6;
        if (!isFirst)
        {
            GameObject p = GameResourcePoolManager._instance.GetPrefabFromKey(DefineUtillHelper.ePrefabType.UI, (int)DefineUtillHelper.ePrefabUIs.LoaddingWnd);
            GameObject go = Instantiate(p, transform);
            _wndLoadding = go.GetComponent<LoaddingWindow>();
            _wndLoadding.OpenWnd();
            _delayTime = 0;
        }
        SoundManager._instance.PlayBGMSound(DefineUtillHelper.eBGMClipKind.Lobby);
    }

    public void StartIngameScene()
    {
        _asyncOper = SceneManager.LoadSceneAsync("IngameScene");
        _nowScene = DefineUtillHelper.eSceneNumber.Ingame;
        if(_wndLoadding == null)
        {
            GameObject p = GameResourcePoolManager._instance.GetPrefabFromKey(DefineUtillHelper.ePrefabType.UI, (int)DefineUtillHelper.ePrefabUIs.LoaddingWnd);
            GameObject go = Instantiate(p, transform);
            _wndLoadding = go.GetComponent<LoaddingWindow>();
            _wndLoadding.OpenWnd();
        }
        _wndLoadding.OpenWnd();
        _delayTime = 0;
        SoundManager._instance.PlayBGMSound(DefineUtillHelper.eBGMClipKind.Map1);
    }
}
