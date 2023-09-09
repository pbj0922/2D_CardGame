using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineUtillHelper
{
    #region [Card Define]
    public const int _minCardPairGenerateCount = 4;
    public const int _maxCardPairGenerateCount = 15;
    public const int _limitHorizCardCount = 6;
    public const int _limitVertzCardCount = 5;

    public enum eCardIconType
    {
        Pikachu         = 0,
        Bulbasaur,
        Charmander,
        Squirtle,
        Venusaur,
        Charizard,
        Blastoise,
        Caterpie,
        Butterfree,
        Weedle,
        Beedrill,
        Pidgey,
        Pidgeot,
        Rattata,
        Spearow,
        Fearow,
        Ekans,
        Arbok,
        Sandshrew,
        NidoranFemale,
        Nidoqueen,
        NidoranMale,
        Nidoking,
        Clefable,
        Vulpix,
        Jigglypuff,
        Golbat,
        Vileplume,
        Parasect,
        Venonat,

        max_IconType
    }
    #endregion

    #region [Avatar Define]
    public enum eAniState
    {
        IDLE            = 0,
        RUN,
        JUMP,
        ATTACK,
        HIT,
        SKILL,
        LETHALMOVE,
        DEAD,

        max_aniCount
    }
    public enum eGradeType
    {
        Normal                  = 0,
        Magic,
        Rare,
        Unique,
        Boss
    }

    #endregion

    #region[Manager Define]
    public enum ePrefabType
    {
        Character               = 0,
        UI,
        Map
    }
    public enum ePrefabChar
    {
        P_Ninja                 = 0,
        M_Wizard
    }
    public enum ePrefabUIs
    {
        LoaddingWnd             = 0,
        StageSlot,
        ResultWnd,
        CardObj
    }
    public enum eSceneNumber
    {
        none                  = 0,
        Lobby,
        Ingame
    }
    public enum eIngameState
    {
        none                   = 0,
        // 게임 일시 정지
        Stop,
        // 게임 준비 단계
        Setting,
        Spawn,
        BeforeStart,
        // 게임 실행 단계
        CardAction,
        RefillCard,
        // 게임 종료 단계
        BeforeEnd,
        End,
        Termination,
        Result
    }
    public enum eBGMClipKind
    {
        Lobby                   = 0,
        Map1,

    }
    public enum eSFXClipKind
    {
        Button_nor              = 0,
        Button_sel,
        Slash_nor,
        Magic_nor,
        Counting,

    }
    public enum eVOICEClipKind
    {
        Num0                    = 0,
        Num1,
        Num2,
        Num3,
        Num4,
        Num5,
        Num6,
        Num7,
        Num8,
        Num9,
        Num10
    }
    #endregion

    #region[UI Define]
    public enum eCountingState
    {
        none                = 0,
        O_Count,
        X_Count,
        CountingEnd
    }

    public enum eStageSlotStage
    {
        Lock                = 0,
        Free,
        Select
    }
    #endregion

    public struct stAvatarInfo
    {
        public string _name;
        public int _lifePower;
        public int _attack;
        public int _deffence;
        public float _accuracyRate;
        public float _avoidanceRate;
        public stAvatarInfo(string n, int l, int a, int d, float acc = 0, float avo = 0)
        {
            _name = n;
            _lifePower = l;
            _attack = a;
            _deffence = d;
            _accuracyRate = acc;
            _avoidanceRate = avo;
        }
    }

    public struct stMonsterInfo
    {
        public int _index;
        public string _name;
        public int _lifePower;
        public int _attack;
        public int _deffence;
        public float _accuracyRate;
        public float _avoidanceRate;
        public int _chanceCount;
        public ePrefabChar _modelType;
        public stMonsterInfo(ePrefabChar t, int no, string n, int l, int a, int d, int c, float acc = 0, float avo = 0)
        {
            _modelType = t;
            _index = no;
            _name = n;
            _lifePower = l;
            _attack = a;
            _deffence = d;
            _chanceCount = c;
            _accuracyRate = acc;
            _avoidanceRate = avo;
        }
    }
    public struct stBattleStageInfo
    {
        public int _stageNumber;
        public string _name;
        public int _pairCount;
        public int[] _monstersIndex;
        public stBattleStageInfo(int stageNO, int pairCnt, string name, params int[] indexes)
        {
            _stageNumber = stageNO;
            _pairCount = pairCnt;
            _name = name;
            _monstersIndex = indexes;
        }
    }
}
