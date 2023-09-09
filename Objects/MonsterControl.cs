using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : UnitBase
{
    [SerializeField] GameObject _prefabHitEffect;
    [SerializeField] Transform _normalHitPos;
    // 기본 정보
    DefineUtillHelper.eGradeType _gType;
    int _chanceCount;

    // 참조형 변수
    SpriteRenderer _model;
    Animator _aniCtrl;
    UnitBase _target;

    // 정보형 변수
    float _movSpeed = 2;
    DefineUtillHelper.eAniState _currentAniState;
    List<Vector3> _ltMonsterSpawnPoints;
    int _directorNextMoveIndex = 0;

    public string _myName
    {
        get { return _name; }
    }
    public override float _hpRate
    {
        get { return _nowHP / (float)_maxHP; }
    }
    
    public DefineUtillHelper.eGradeType type
    {
        get { return _gType; }
    }
    public int _limitAcount
    {
        get { return _chanceCount; }
    }


    void Awake()
    {
        _aniCtrl = GetComponent<Animator>();
        _model = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_isDead)
        {
            return;
        }
        if (!_isEndDirector)
        {
            if (_directorNextMoveIndex < _ltMonsterSpawnPoints.Count)
            {
                Vector3 goal = _ltMonsterSpawnPoints[_directorNextMoveIndex];
                transform.position = Vector3.MoveTowards(transform.position, goal, _movSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, goal) <= 0.1f)
                {
                    transform.position = goal;
                    _directorNextMoveIndex++;
                }
            }
            else
            {
                ChangeAnimationFromAction(DefineUtillHelper.eAniState.IDLE);
                _isEndDirector = true;
            }
        }
    }

    public void InitDataSet(List<Vector3> lt, DefineUtillHelper.stMonsterInfo info, DefineUtillHelper.eGradeType gtype)
    {
        // 기본 정보
        _name = info._name;
        _maxHP = _nowHP = info._lifePower;
        _att = info._attack;
        _def = info._deffence;
        _acc = info._accuracyRate;
        _avo = info._avoidanceRate;
        _gType = gtype;
        _chanceCount = info._chanceCount;
        SetStatWeightByGrade(_gType);

        // 위치 정보
        _ltMonsterSpawnPoints = lt;
        _directorNextMoveIndex++;
        ChangeAnimationFromAction(DefineUtillHelper.eAniState.RUN);
    }

    public void ChangeAnimationFromAction(DefineUtillHelper.eAniState state)
    {
        switch (state)
        {
            case DefineUtillHelper.eAniState.IDLE:
                _aniCtrl.SetBool("IsRun", false);
                break;
            case DefineUtillHelper.eAniState.RUN:
                _aniCtrl.SetBool("IsRun", true);
                break;
            case DefineUtillHelper.eAniState.HIT:
                _aniCtrl.SetTrigger("Hitting");
                break;
            case DefineUtillHelper.eAniState.ATTACK:
                _aniCtrl.SetTrigger("Attack");
                break;
            case DefineUtillHelper.eAniState.DEAD:
                _isDead = true;
                _aniCtrl.SetBool("IsDead", _isDead);
                break;
        }
        _currentAniState = state;
    }

    public void AnimationEvenetFunc()
    {
        _target.HittingMe(_att, _acc);
        IngameManger._instance._isEndOfDirecting = true;
    }

    public override void Attack(UnitBase target)
    {
        ChangeAnimationFromAction(DefineUtillHelper.eAniState.ATTACK);
        _target = target;
        SoundManager._instance.PlaySFXSound(DefineUtillHelper.eSFXClipKind.Magic_nor);
    }

    public override bool HittingMe(int damage, float acc)
    {
        GameObject go = Instantiate(_prefabHitEffect, _normalHitPos.position, _prefabHitEffect.transform.rotation);
        go.transform.localScale *= 0.8f;

        int finishDamage = damage - _def;
        finishDamage = (finishDamage < 1) ? 1 : finishDamage;
        if((_nowHP -= finishDamage) <= 0)
        {
            _nowHP = 0;

            // 죽는 처리....
            ChangeAnimationFromAction(DefineUtillHelper.eAniState.DEAD);
        }
        else
        {
            ChangeAnimationFromAction(DefineUtillHelper.eAniState.HIT);
        }
        IngameManger._instance.SettingMonsterDisplay();

        return _isDead;
    }

    void SetStatWeightByGrade(DefineUtillHelper.eGradeType gtype)
    {
        switch(gtype)
        {
            case DefineUtillHelper.eGradeType.Magic:
                ApplyCalculation(1.2f);
                break;
            case DefineUtillHelper.eGradeType.Rare:
                ApplyCalculation(2);
                break;
            case DefineUtillHelper.eGradeType.Unique:
                ApplyCalculation(3);
                break;
            case DefineUtillHelper.eGradeType.Boss:
                ApplyCalculation(5);
                break;
        }
    }
    void ApplyCalculation(float magnification)
    {
        _nowHP = _maxHP = (int)(_maxHP * (magnification * 2));
        _att = (int)(_att * magnification);
        _def = (int)(_def * magnification);
    }
}
