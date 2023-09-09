using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    protected string _name;
    protected int _att;
    protected int _def;
    protected int _maxHP;
    protected int _nowHP;
    protected float _acc;
    protected float _avo;

    /// <summary>
    /// 연출 종료시 true를 반환.
    /// </summary>
    public bool _isEndDirector
    {
        get; set;
    }
    /// <summary>
    /// 죽으면 true, 살아 있으면 false 이다.
    /// </summary>
    public bool _isDead
    {
        get; set;
    }

    abstract public float _hpRate
    {
        get;
    }

    /// <summary>
    /// 피격시 호출되어 데미지 계산을 한다
    /// </summary>
    /// <param name="damage">공격자의 데미지</param>
    /// <param name="acc">공격자의 명중률</param>
    /// <returns>true를 반환하면 죽은 것임</returns>
    abstract public bool HittingMe(int damage, float acc);

    abstract public void Attack(UnitBase target);
}
