using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResourcePoolManager : MonoBehaviour
{
    static GameResourcePoolManager _uniqueInstance;

    [SerializeField] GameObject[] _prefabChars;
    [SerializeField] GameObject[] _prefabUIs;
    [SerializeField] GameObject[] _prefabMaps;
    [SerializeField] AudioClip[] _bgms;
    [SerializeField] AudioClip[] _sfxs;
    [SerializeField] AudioClip[] _voices;
    [SerializeField] Sprite[] _cardImages;
    [SerializeField] Sprite[] _gradeImages;

    public static GameResourcePoolManager _instance
    {
        get { return _uniqueInstance; }
    }

    // �������� ���� �����
    Dictionary<int, DefineUtillHelper.stBattleStageInfo> _stageInfoTable = new Dictionary<int, DefineUtillHelper.stBattleStageInfo>();
    // ���� ���� �����
    Dictionary<int, DefineUtillHelper.stMonsterInfo> _monInfoTable = new Dictionary<int, DefineUtillHelper.stMonsterInfo>();

    void Awake()
    {
        _uniqueInstance = this;
        DontDestroyOnLoad(gameObject);

        // �ӽ�
        DummyStageDataSamples();
        DummyMonsterDataSamples();
        //===
    }

    public GameObject GetPrefabFromKey(DefineUtillHelper.ePrefabType key, int index)
    {
        GameObject re = null;
        switch(key)
        {
            case DefineUtillHelper.ePrefabType.Character:
                re = _prefabChars[index];
                break;
            case DefineUtillHelper.ePrefabType.UI:
                re = _prefabUIs[index];
                break;
            case DefineUtillHelper.ePrefabType.Map:
                re = _prefabMaps[index - 1];
                break;
        }

        return re;
    }

    public AudioClip GetBGMClipFrom(DefineUtillHelper.eBGMClipKind kind)
    {
        return _bgms[(int)kind];
    }
    public AudioClip GetSFXClipFrom(DefineUtillHelper.eSFXClipKind kind)
    {
        return _sfxs[(int)kind];
    }
    public AudioClip GetVOICEClipFrom(DefineUtillHelper.eVOICEClipKind kind)
    {
        return _voices[(int)kind];
    }

    public DefineUtillHelper.stBattleStageInfo GetStageInfoFrom(int stageNum)
    {
        return _stageInfoTable[stageNum];
    }

    public DefineUtillHelper.stMonsterInfo GetMonsterInfoFrom(int monID)
    {
        return _monInfoTable[monID];
    }

    public Sprite GetCardImageFrom(DefineUtillHelper.eCardIconType type)
    {
        return _cardImages[(int)type];
    }

    public Sprite GetGradeImageFrom(DefineUtillHelper.eGradeType type)
    {
        return _gradeImages[(int)type];
    }

    void DummyStageDataSamples()
    {
        DefineUtillHelper.stBattleStageInfo stageInfo = new DefineUtillHelper.stBattleStageInfo(1, 4, "ù ��!", 1003);
        _stageInfoTable.Add(stageInfo._stageNumber, stageInfo);
        stageInfo = new DefineUtillHelper.stBattleStageInfo(2, 4, "�ο��°�־ֿ���", 1003, 1001);
        _stageInfoTable.Add(stageInfo._stageNumber, stageInfo);
        stageInfo = new DefineUtillHelper.stBattleStageInfo(3, 6, "���� �� �˰ھ�", 1003, 1003, 1001);
        _stageInfoTable.Add(stageInfo._stageNumber, stageInfo);
        stageInfo = new DefineUtillHelper.stBattleStageInfo(4, 5, "���� ���� �� ������...", 1001, 1003, 1001);
        _stageInfoTable.Add(stageInfo._stageNumber, stageInfo);
    }

    void DummyMonsterDataSamples()
    {
        //_dummyMonInfoTable
        DefineUtillHelper.stMonsterInfo info = new DefineUtillHelper.stMonsterInfo(DefineUtillHelper.ePrefabChar.M_Wizard, 1001, "���ų��� ������", 50, 6, 0, 3);
        _monInfoTable.Add(info._index, info);
        info = new DefineUtillHelper.stMonsterInfo(DefineUtillHelper.ePrefabChar.M_Wizard, 1002, "��ġ ���� ����", 60, 5, 2, 3);
        _monInfoTable.Add(info._index, info);
        info = new DefineUtillHelper.stMonsterInfo(DefineUtillHelper.ePrefabChar.M_Wizard, 1003, "���ֺ��� ����", 30, 3, 2, 3);
        _monInfoTable.Add(info._index, info);
        info = new DefineUtillHelper.stMonsterInfo(DefineUtillHelper.ePrefabChar.M_Wizard, 1004, "�꺸�̴� ������", 65, 7, 1, 3);
        _monInfoTable.Add(info._index, info);
    }
}
