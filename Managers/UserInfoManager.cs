using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoManager : MonoBehaviour
{
    static UserInfoManager _uniqueInstance;

    public int _clearStageNumber
    {
        get;set;
    }

    public int _nowStageToProceed
    {
        get;set;
    }

    public static UserInfoManager _instance
    {
        get { return _uniqueInstance; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        DontDestroyOnLoad(gameObject);

        _clearStageNumber = 0;
        _nowStageToProceed = 1;
    }
}
