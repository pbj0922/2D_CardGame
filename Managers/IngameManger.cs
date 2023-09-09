using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameManger : MonoBehaviour
{
    static IngameManger _uniqueInstance;

    // ������ ����
    AvatarControl _playerA;
    MonsterControl _nowMonster;
    CardGenerator _cardGen;
    MessageBox _msgMainBox;
    MiniCharacterBox _miniAvaBox;
    MiniMonsterBox _miniMonBox;
    TimerBox _timerBox;
    MenuBox _menuBox;

    // ������ ����
    float _startDelayTime = 1;
    float _clearDelayTime = 2;
    List<Vector3> _ltAvatarSpawnPoints;
    List<Vector3> _ltMonsterSpawnPoints;
    //int _countMonsterAppearance = 1;
    DefineUtillHelper.eIngameState _currentFlowState = DefineUtillHelper.eIngameState.none;
    float _checkTime = 0;
    // ��Ʋ ���� ����
    bool _isRefill = false;
    List<int> _battleMonsterList;
    int _nowBattleIndex = 0;
    int _chanceAcount = 0;
    Queue<int> _cardIndexis;
    bool _isSuccess = false;
    int _cardActionTime = 4;
    int _pairCount = 0;                                  // ī����� ����
    int _correctCount = 0;
    int _wrongCount = 0;

    public DefineUtillHelper.eIngameState _nowFlowState
    {
        get { return _currentFlowState; }
    }

    public bool _isEndOfDirecting
    {
        get; set;
    }

    public static IngameManger _instance
    {
        get { return _uniqueInstance; }
    }

    void Awake()
    {
        _uniqueInstance = this;

        _ltAvatarSpawnPoints = new List<Vector3>();
        _ltMonsterSpawnPoints = new List<Vector3>();
    }

    void Start()
    {
        // �ӽ�
        //DefineUtillHelper.stBattleStageInfo stageInfo = new DefineUtillHelper.stBattleStageInfo(1, 4, "ù ��!", 1001);
        //DefineUtillHelper.stBattleStageInfo stageInfo = GameResourcePoolManager._instance.GetStageInfoFrom(UserInfoManager._instance._nowStageToProceed);
        //InitalizeSetting(stageInfo);
        //SpawnPlayer();
        //SpawnMonster();
        //===
    }

    void Update()
    {
        switch(_currentFlowState)
        {
            case DefineUtillHelper.eIngameState.BeforeStart:
                _checkTime += Time.deltaTime;
                if(_checkTime >= _startDelayTime)
                {
                    // CardAction�ܰ�� �Ѿ�� ��.
                    CardPlayGame();
                }
                break;
            case DefineUtillHelper.eIngameState.CardAction:
                {
                    _isRefill = CardMatchingProcess();
                    if (_isEndOfDirecting)
                    {
                        if (_playerA._isDead)
                        {
                            _isRefill = false;
                            BeforeEndGame(false);
                        }
                        else if (_nowMonster._isDead && _nowBattleIndex == _battleMonsterList.Count)
                        {
                            _isRefill = false;
                            BeforeEndGame(true);
                        }
                        if (_isRefill)
                        {
                            // ī�� ����
                            RefillCardGame();
                        }

                        _isEndOfDirecting = false;
                    }
                }
                break;
            case DefineUtillHelper.eIngameState.RefillCard:
                if(_cardGen._checkEnd)
                {
                    CardPlayGame();
                }
                break;
            case DefineUtillHelper.eIngameState.BeforeEnd:
                _checkTime += Time.deltaTime;
                if (_checkTime >= _clearDelayTime)
                {
                    EndGame();
                }
                break;
            case DefineUtillHelper.eIngameState.End:
                _checkTime -= Time.deltaTime;
                if(_checkTime <= 0)
                {
                    ResultGame();
                }
                break;
        }
    }



    /// <summary>
    /// IngamManager�� �⺻�������� ���� ������ �����ϴ� �Լ�.
    /// </summary>
    public void InitalizeSetting(DefineUtillHelper.stBattleStageInfo stageInfo)
    {
        _currentFlowState = DefineUtillHelper.eIngameState.Setting;

        _pairCount = stageInfo._pairCount;
        _battleMonsterList = new List<int>();
        _cardIndexis = new Queue<int>();
        _cardGen = GetComponent<CardGenerator>();
        GameObject go = GameObject.FindGameObjectWithTag("AvatarSpawnPoint");
        if(go != null)
        {
            Transform[] points = go.GetComponentsInChildren<Transform>();
            for(int n = 0; n < points.Length; n++)
            {
                _ltAvatarSpawnPoints.Add(points[n].position);
            }
        }
        go = GameObject.FindGameObjectWithTag("MonsterSpawnPoint");
        if(go != null)
        {
            Transform[] points = go.GetComponentsInChildren<Transform>();
            for (int n = 0; n < points.Length; n++)
            {
                _ltMonsterSpawnPoints.Add(points[n].position);
            }
        }
        go = GameObject.FindGameObjectWithTag("UIMessageBox");
        _msgMainBox = go.GetComponent<MessageBox>();
        go = GameObject.FindGameObjectWithTag("UIMiniAvatarBox");
        _miniAvaBox = go.GetComponent<MiniCharacterBox>();
        go = GameObject.FindGameObjectWithTag("UIMiniMonsterBox");
        _miniMonBox = go.GetComponent<MiniMonsterBox>();
        go = GameObject.FindGameObjectWithTag("UITimerBox");
        _timerBox = go.GetComponent<TimerBox>();
        go = GameObject.FindGameObjectWithTag("UIMenuBox");
        _menuBox = go.GetComponent<MenuBox>();

        _menuBox.CloseBox();
        _msgMainBox.OpenMessageBox("READY~~");
        _miniAvaBox.gameObject.SetActive(false);
        _miniMonBox.OffView();
        for (int n = 0; n < stageInfo._monstersIndex.Length; n++)
        {
            _battleMonsterList.Add(stageInfo._monstersIndex[n]);
        }

        _timerBox.InitData(_cardActionTime);
        StartCoroutine(SpawnGameObjects());
    }

    /// <summary>
    /// �ƹ�Ÿ, ����, ī�带 �������Ѽ� �ڸ��� ��� �ϴ� �Լ�.
    /// </summary>
    public IEnumerator SpawnGameObjects()
    {
        yield return new WaitForSeconds(2);
        _currentFlowState = DefineUtillHelper.eIngameState.Spawn;
        // ���ΰ� �ƹ�Ÿ ����
        SpawnPlayer();
        _msgMainBox.OpenMessageBox("���ΰ� ��!��!");
        while(_playerA == null)
        {
            yield return null;
        }
        while(!_playerA._isEndDirector)
        {
            yield return null;
        }
        // ���� ����
        SpawnMonster();
        _msgMainBox.OpenMessageBox("��� ���� ����~");
        while (_nowMonster == null)
        {
            yield return null;
        }
        while (!_nowMonster._isEndDirector)
        {
            yield return null;
        }
        // ī�� ����
        _msgMainBox.OpenMessageBox("��Ʋ ī�� ����!!");
        _cardGen.StartGenerate(_pairCount);
        while(!_cardGen._checkEnd)
        {
            yield return null;
        }

        // FlowState�� BeforeStart�� ����
        StartGame();
    }

    /// <summary>
    /// ���� ������ ��� ���� ������ ��Ű�� �Լ�
    /// </summary>
    public void StartGame()
    {
        _currentFlowState = DefineUtillHelper.eIngameState.BeforeStart;

        _msgMainBox.OpenMessageBox("Game Start~!");
    }

    /// <summary>
    /// ī�� �÷��̸� �����ϰ� �ϴ� �Լ�
    /// </summary>
    public void CardPlayGame()
    {
        _currentFlowState = DefineUtillHelper.eIngameState.CardAction;

        _msgMainBox.CloseBox();
        _timerBox.ResetTime();
    }

    public void RefillCardGame()
    {
        _currentFlowState = DefineUtillHelper.eIngameState.RefillCard;

        _msgMainBox.OpenMessageBox("��Ʋ ī�� �ٽ� ����!!");
        _cardGen.ListAllClear();
        _cardGen.StartGenerate(_pairCount);
    }

    /// <summary>
    /// ���� ����� ȣ��. �÷��̾� �ƹ�Ÿ ����� failed�� ���� ��� óġ�� true�� ���´�.
    /// </summary>
    /// <param name="isSuccess">stage ���� ���� ����</param>
    public void BeforeEndGame(bool isSuccess)
    {
        _currentFlowState = DefineUtillHelper.eIngameState.BeforeEnd;

        _isSuccess = isSuccess;
        _checkTime = 0;
    }

    /// <summary>
    /// ������ ������ ������ ������ �Բ� ����...
    /// </summary>
    public void EndGame()
    {
        _currentFlowState = DefineUtillHelper.eIngameState.End;

        _checkTime = 3;
        if (_isSuccess)
        {
            _msgMainBox.OpenMessageBox("Game Clear!!!!");
        }
        else
        {
            _msgMainBox.OpenMessageBox("Game Over....");
        }
    }

    public void ResultGame()
    {
        _currentFlowState = DefineUtillHelper.eIngameState.Result;

        _msgMainBox.CloseBox();

        // ���â ����
        GameObject prefab = GameResourcePoolManager._instance.GetPrefabFromKey(DefineUtillHelper.ePrefabType.UI, (int)DefineUtillHelper.ePrefabUIs.ResultWnd);
        GameObject go = Instantiate(prefab);
        ResultWindow wnd = go.GetComponent<ResultWindow>();
        wnd.OpenWindow(_isSuccess, _correctCount, _wrongCount);
        _correctCount = 0;
        _wrongCount = 0;
    }


    void SpawnPlayer()
    {
        GameObject prefab = GameResourcePoolManager._instance.GetPrefabFromKey(DefineUtillHelper.ePrefabType.Character, (int)DefineUtillHelper.ePrefabChar.P_Ninja);
        GameObject go = Instantiate(prefab, _ltAvatarSpawnPoints[0], Quaternion.identity);
        _playerA = go.GetComponent<AvatarControl>();
        // �ӽ�
        DefineUtillHelper.stAvatarInfo info = new DefineUtillHelper.stAvatarInfo("ȫ�浿", 100, 10, 3);
        //===
        _playerA.InitAvatarDataSet(_ltAvatarSpawnPoints, info);
        _miniAvaBox.InitDataSet(_playerA._myName);
    }
    void SpawnMonster()
    {
        int nowID = _battleMonsterList[_nowBattleIndex++];
        DefineUtillHelper.stMonsterInfo monInfo = GameResourcePoolManager._instance.GetMonsterInfoFrom(nowID);
        GameObject prefab = GameResourcePoolManager._instance.GetPrefabFromKey(DefineUtillHelper.ePrefabType.Character, (int)monInfo._modelType);

        GameObject go = Instantiate(prefab, _ltMonsterSpawnPoints[0], Quaternion.identity);
        _nowMonster = go.GetComponent<MonsterControl>();
        _nowMonster.InitDataSet(_ltMonsterSpawnPoints, monInfo, DefineUtillHelper.eGradeType.Normal);
        Sprite gradeIcon = GameResourcePoolManager._instance.GetGradeImageFrom(_nowMonster.type);
        _miniMonBox.InitDataSet(gradeIcon, _nowMonster._myName, monInfo._chanceCount);
        //_miniMonBox.MoveCountBox();
    }
    bool CardMatchingProcess()              // return true�� ī�� ����.
    {
        if(_cardIndexis.Count != 0 && _cardIndexis.Count % 2 == 0)
        {
            // ī�� ��Ī�� �Ѵ�.
            CardControl first = _cardGen[_cardIndexis.Dequeue() - 1];
            CardControl second = _cardGen[_cardIndexis.Dequeue() - 1];

            if(first._iType == second._iType)
            {
                // ī�带 �����
                Destroy(first.gameObject);
                Destroy(second.gameObject);
                // �ƹ�Ÿ ���ݷ����� ���� �ǰ�
                _playerA.Attack(_nowMonster);
                // ���� Ƚ�� ����
                _correctCount += 1;
            }
            else
            {
                // ī�带 ������ ������ chance acount�� �ø���
                first.Reverse();
                second.Reverse();
                _chanceAcount++;
                if (_chanceAcount == _nowMonster._limitAcount)
                {
                    _chanceAcount = 0;
                    // ���� ����
                    // ���� ���ݷ����� �ƹ�Ÿ �ǰ�
                    _nowMonster.Attack(_playerA);
                }
                _miniMonBox.SetRemainCount(_nowMonster._limitAcount - _chanceAcount);
                // Ʋ�� Ƚ�� ����
                _wrongCount += 1;
            }
            _timerBox.ResetTime();
        }
        // ī�带 �� ������ Ȯ��
        bool isAllUse = true;
        for(int n =0; n < _cardGen._cardCount; n++)
        {
            if (_cardGen[n] != null)
            {
                isAllUse = false;
            }
        }
        return isAllUse;
    }

    public void SelectCard(int cardID)
    {
        // ó���� ī�� ��ȣ ����
        // �ι�°�� ī�� ��ȣ���� ��
        _cardIndexis.Enqueue(cardID);
    }

    public void CouldntChoose()
    {
        if(_cardIndexis.Count > 0)
        {
            _cardGen[_cardIndexis.Dequeue() - 1].Reverse();
        }

        _chanceAcount++;
        if(_chanceAcount == _nowMonster._limitAcount)
        {
            _chanceAcount = 0;
            _nowMonster.Attack(_playerA);
        }
        _miniMonBox.SetRemainCount(_nowMonster._limitAcount - _chanceAcount);
        _timerBox.ResetTime();
    }

    public void SettingAvatarDisplay()
    {
        _miniAvaBox.SetHPRate(_playerA._hpRate);
    }

    public void SettingMonsterDisplay()
    {
        _miniMonBox.SetHPRate(_nowMonster._hpRate);
    }

    public void ClickMenuButton()
    {
        _menuBox.OpenBox();
    }

    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0, 0, 400, 500), "Reverse"))
    //    {
    //        for(int n = 0; n < _cardGen._cardCount; n++)
    //        {
    //            _cardGen[n].Reverse();
    //        }
    //    }
    //}
}
