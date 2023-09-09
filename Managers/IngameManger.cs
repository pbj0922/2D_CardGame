using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameManger : MonoBehaviour
{
    static IngameManger _uniqueInstance;

    // 참조형 변수
    AvatarControl _playerA;
    MonsterControl _nowMonster;
    CardGenerator _cardGen;
    MessageBox _msgMainBox;
    MiniCharacterBox _miniAvaBox;
    MiniMonsterBox _miniMonBox;
    TimerBox _timerBox;
    MenuBox _menuBox;

    // 정보형 변수
    float _startDelayTime = 1;
    float _clearDelayTime = 2;
    List<Vector3> _ltAvatarSpawnPoints;
    List<Vector3> _ltMonsterSpawnPoints;
    //int _countMonsterAppearance = 1;
    DefineUtillHelper.eIngameState _currentFlowState = DefineUtillHelper.eIngameState.none;
    float _checkTime = 0;
    // 배틀 관련 정보
    bool _isRefill = false;
    List<int> _battleMonsterList;
    int _nowBattleIndex = 0;
    int _chanceAcount = 0;
    Queue<int> _cardIndexis;
    bool _isSuccess = false;
    int _cardActionTime = 4;
    int _pairCount = 0;                                  // 카드쌍의 갯수
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
        // 임시
        //DefineUtillHelper.stBattleStageInfo stageInfo = new DefineUtillHelper.stBattleStageInfo(1, 4, "첫 쌈!", 1001);
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
                    // CardAction단계로 넘어가게 함.
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
                            // 카드 리필
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
    /// IngamManager의 기본정보값과 참조 정보를 설정하는 함수.
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
    /// 아바타, 몬스터, 카드를 생성시켜서 자리를 잡게 하는 함수.
    /// </summary>
    public IEnumerator SpawnGameObjects()
    {
        yield return new WaitForSeconds(2);
        _currentFlowState = DefineUtillHelper.eIngameState.Spawn;
        // 주인공 아바타 등장
        SpawnPlayer();
        _msgMainBox.OpenMessageBox("주인공 등!장!");
        while(_playerA == null)
        {
            yield return null;
        }
        while(!_playerA._isEndDirector)
        {
            yield return null;
        }
        // 몬스터 등장
        SpawnMonster();
        _msgMainBox.OpenMessageBox("상대 몬스터 등장~");
        while (_nowMonster == null)
        {
            yield return null;
        }
        while (!_nowMonster._isEndDirector)
        {
            yield return null;
        }
        // 카드 생성
        _msgMainBox.OpenMessageBox("배틀 카드 생성!!");
        _cardGen.StartGenerate(_pairCount);
        while(!_cardGen._checkEnd)
        {
            yield return null;
        }

        // FlowState를 BeforeStart로 변경
        StartGame();
    }

    /// <summary>
    /// 게임 시작전 잠시 동안 딜레이 시키는 함수
    /// </summary>
    public void StartGame()
    {
        _currentFlowState = DefineUtillHelper.eIngameState.BeforeStart;

        _msgMainBox.OpenMessageBox("Game Start~!");
    }

    /// <summary>
    /// 카드 플레이를 시작하게 하는 함수
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

        _msgMainBox.OpenMessageBox("배틀 카드 다시 생성!!");
        _cardGen.ListAllClear();
        _cardGen.StartGenerate(_pairCount);
    }

    /// <summary>
    /// 게임 종료시 호출. 플레이어 아바타 사망시 failed가 몬스터 모두 처치시 true가 들어온다.
    /// </summary>
    /// <param name="isSuccess">stage 성골 실패 여부</param>
    public void BeforeEndGame(bool isSuccess)
    {
        _currentFlowState = DefineUtillHelper.eIngameState.BeforeEnd;

        _isSuccess = isSuccess;
        _checkTime = 0;
    }

    /// <summary>
    /// 게임이 완전히 끝나고 문구와 함께 여운...
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

        // 결과창 등장
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
        // 임시
        DefineUtillHelper.stAvatarInfo info = new DefineUtillHelper.stAvatarInfo("홍길동", 100, 10, 3);
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
    bool CardMatchingProcess()              // return true면 카드 리필.
    {
        if(_cardIndexis.Count != 0 && _cardIndexis.Count % 2 == 0)
        {
            // 카드 매칭을 한다.
            CardControl first = _cardGen[_cardIndexis.Dequeue() - 1];
            CardControl second = _cardGen[_cardIndexis.Dequeue() - 1];

            if(first._iType == second._iType)
            {
                // 카드를 지운다
                Destroy(first.gameObject);
                Destroy(second.gameObject);
                // 아바타 공격력으로 몬스터 피격
                _playerA.Attack(_nowMonster);
                // 맞춘 횟수 증가
                _correctCount += 1;
            }
            else
            {
                // 카드를 뒤집고 몬스터의 chance acount를 늘린다
                first.Reverse();
                second.Reverse();
                _chanceAcount++;
                if (_chanceAcount == _nowMonster._limitAcount)
                {
                    _chanceAcount = 0;
                    // 몬스터 공격
                    // 몬스터 공격력으로 아바타 피격
                    _nowMonster.Attack(_playerA);
                }
                _miniMonBox.SetRemainCount(_nowMonster._limitAcount - _chanceAcount);
                // 틀린 횟수 증가
                _wrongCount += 1;
            }
            _timerBox.ResetTime();
        }
        // 카드를 다 썻는지 확인
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
        // 처음엔 카드 번호 저장
        // 두번째는 카드 번호끼리 비교
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
