using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarControl : UnitBase
{
    [SerializeField] GameObject _prefabHitEffect;
    [SerializeField] Transform _normalHitPos;
    // 참조형 변수
    SpriteRenderer _model;
    Animator _aniCtrl;
    UnitBase _target;

    // 정보형 변수
    float _movSpeed = 3;
    DefineUtillHelper.eAniState _currentAniState;
    List<Vector3> _ltAvatarSpawnPoints;
    int _directorNextMoveIndex = 0;

    public string _myName
    {
        get { return _name; }
    }

    public override float _hpRate
    {
        get { return _nowHP / (float)_maxHP; }
    }

    void Awake()
    {
        _aniCtrl = GetComponent<Animator>();
        _model = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(_isDead)
        {
            return;
        }
        if (!_isEndDirector)
        {
            if (_directorNextMoveIndex < _ltAvatarSpawnPoints.Count)
            {
                Vector3 goal = _ltAvatarSpawnPoints[_directorNextMoveIndex];
                transform.position = Vector3.MoveTowards(transform.position, goal, _movSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, goal) <= 0.1f)
                {
                    transform.position = goal;
                    _directorNextMoveIndex++;
                    _model.flipX = !_model.flipX;
                }
            }
            else
            {
                ChangeAnimationFromAction(DefineUtillHelper.eAniState.IDLE);
                _isEndDirector = true;
            }
        }
    }

    public void InitAvatarDataSet(List<Vector3> lt, DefineUtillHelper.stAvatarInfo info)
    {
        // 기본 정보
        _name = info._name;
        _maxHP = _nowHP = info._lifePower;
        _att = info._attack;
        _def = info._deffence;
        _acc = info._accuracyRate;
        _avo = info._avoidanceRate;

        // 이동 정보
        _ltAvatarSpawnPoints = lt;
        _directorNextMoveIndex++;
        ChangeAnimationFromAction(DefineUtillHelper.eAniState.RUN);
    }

    public void ChangeAnimationFromAction(DefineUtillHelper.eAniState state)
    {
        switch(state)
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
        //Debug.Log("AnimationEvenetFunc called");
        IngameManger._instance._isEndOfDirecting = true;
    }

    public override void Attack(UnitBase target)
    {
        // 에니메이션만 바뀜
        ChangeAnimationFromAction(DefineUtillHelper.eAniState.ATTACK);
        _target = target;
        SoundManager._instance.PlaySFXSound(DefineUtillHelper.eSFXClipKind.Slash_nor);
    }

    public override bool HittingMe(int damage, float acc)
    {
        GameObject go = Instantiate(_prefabHitEffect, _normalHitPos.position, _prefabHitEffect.transform.rotation);
        go.transform.localScale *= 0.8f;

        int finishDamage = damage - _def;
        finishDamage = (finishDamage < 1) ? 1 : finishDamage;
        if ((_nowHP -= finishDamage) <= 0)
        {
            _nowHP = 0;

            ChangeAnimationFromAction(DefineUtillHelper.eAniState.DEAD);
        }
        else
        {
            ChangeAnimationFromAction(DefineUtillHelper.eAniState.HIT);
        }
        IngameManger._instance.SettingAvatarDisplay();

        return _isDead;
    }

    // void OnGUI()
    //{
    //    if(GUI.Button(new Rect(0,0,300,70), "IDLE"))
    //    {
    //        _aniCtrl.SetBool("IsRun", false);
    //    }
    //    if (GUI.Button(new Rect(0, 70, 300, 70), "RUN"))
    //    {
    //        _aniCtrl.SetBool("IsRun", true);
    //    }
    //    if (GUI.Button(new Rect(0, 140, 300, 70), "HIT"))
    //    {
    //        _aniCtrl.SetTrigger("Hitting");
    //    }
    //    if (GUI.Button(new Rect(0, 210, 300, 70), "ATTACK"))
    //    {
    //        _aniCtrl.SetTrigger("Attack");
    //    }
    //}
}
