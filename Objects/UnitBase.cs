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
    /// ���� ����� true�� ��ȯ.
    /// </summary>
    public bool _isEndDirector
    {
        get; set;
    }
    /// <summary>
    /// ������ true, ��� ������ false �̴�.
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
    /// �ǰݽ� ȣ��Ǿ� ������ ����� �Ѵ�
    /// </summary>
    /// <param name="damage">�������� ������</param>
    /// <param name="acc">�������� ���߷�</param>
    /// <returns>true�� ��ȯ�ϸ� ���� ����</returns>
    abstract public bool HittingMe(int damage, float acc);

    abstract public void Attack(UnitBase target);
}
