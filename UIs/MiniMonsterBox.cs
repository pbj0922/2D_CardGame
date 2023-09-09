using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMonsterBox : MonoBehaviour
{
    //[SerializeField] Image _monsterIcon;
    [SerializeField] Image _rankIcon;
    [SerializeField] Text _monsterName;
    [SerializeField] Text _txtChanceCount;
    [SerializeField] Slider _hpBar;
    [SerializeField] RectTransform _remainCount;
    [SerializeField] Transform _endPosition;

    Text _txtRemain;

    void Start()
    {

    }

    public void InitDataSet(/*Sprite mon, */Sprite rank, string name, int cCount)
    {
        _txtRemain = _remainCount.GetChild(0).GetComponent<Text>();
        gameObject.SetActive(true);
        _remainCount.gameObject.SetActive(true);
        //_monsterIcon.sprite = mon;
        _rankIcon.sprite = rank;
        _monsterName.text = name;
        _txtRemain.text = _txtChanceCount.text = cCount.ToString();
        _hpBar.value = 1;

        iTween.MoveTo(_remainCount.gameObject, iTween.Hash("position", _endPosition.position, "time", 2, "easetype", iTween.EaseType.easeOutBounce));
    }

    //public void MoveCountBox()
    //{
    //    iTween.MoveTo(_remainCount.gameObject, iTween.Hash("position", _endPosition.position, "time", 3, "delay", 1, "easetype", iTween.EaseType.easeOutBounce));
    //}

    public void OffView()
    {
        gameObject.SetActive(false);
        _remainCount.gameObject.SetActive(false);
    }

    public void SetHPRate(float rate)
    {
        _hpBar.value = rate;
    }

    public void SetRemainCount(int cnt)
    {
        _txtRemain.text = cnt.ToString();
    }
}
