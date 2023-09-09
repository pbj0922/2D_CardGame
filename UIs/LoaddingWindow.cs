using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoaddingWindow : MonoBehaviour
{
    [SerializeField] Image _loaddingIcon;
    [SerializeField] Text _txtloadding;

    float _checkTime = 0;
    float _passTime = 0;
    int _count = 0;
    int _countTxt = 0;

    public void OpenWnd()
    {
        gameObject.SetActive(true);
        _txtloadding.text = "Loadding";
        _passTime = _checkTime = 0;
        _countTxt = _count = 0;
    }

    public void CloseWnd()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        _checkTime += Time.deltaTime;
        _passTime += Time.deltaTime;
        if (_checkTime >= 0.1f)
        {
            _checkTime = 0;
            ++_count;
            _count %= 12;
            Quaternion target = Quaternion.Euler(0, 0, _count * -30);
            _loaddingIcon.transform.rotation = target;
        }
        if(_passTime >= 0.8f)
        {
            _passTime = 0;
            _countTxt = ++_countTxt % 7;
            _txtloadding.text = "Loadding";
            for(int n = 0; n < _countTxt; n++)
            {
                _txtloadding.text += ".";
            }
        }
    }
}
