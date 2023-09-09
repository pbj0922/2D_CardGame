using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniCharacterBox : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Slider _hpBar;
    
    public void InitDataSet(string name)
    {
        gameObject.SetActive(true);
        _name.text = name;
        _hpBar.value = 1;
    }

    public void SetHPRate(float rate)
    {
        _hpBar.value = rate;
    }
}
