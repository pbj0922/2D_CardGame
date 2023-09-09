using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBox : MonoBehaviour
{
    [SerializeField] Text _timeText;

    float _setTime;
    float _passTime;
    bool _isRun;


    void Update()
    {
        if (!_isRun)
        {
            return;
        }
        if (IngameManger._instance._nowFlowState == DefineUtillHelper.eIngameState.CardAction)
        {
            _passTime -= Time.deltaTime;
            if (_passTime <= 0)
            {
                TimeOverAt();
            }
            _timeText.text = ((int)_passTime).ToString();
        }
    }

    public void InitData(float time)
    {
        _passTime = _setTime = time;
        _timeText.text = ((int)_passTime).ToString();
    }

    public void ResetTime()
    {
        _passTime = _setTime;
        _isRun = true;
        _timeText.text = ((int)_passTime).ToString();
    }

    void TimeOverAt()
    {
        _passTime = 0;
        _isRun = false;
        _timeText.text = ((int)_passTime).ToString();
        // IngameManager¿¡ ¾Ë¸²
        IngameManger._instance.CouldntChoose();
    }
}