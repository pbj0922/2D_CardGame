using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardControl : MonoBehaviour
{
    [SerializeField] SpriteRenderer _icon;

    // 참조형 변수
    GameObject _frontBG;
    GameObject _backBG;
    SpriteRenderer _backImage;
    // 정보형 변수
    int _no;
    bool _isRot;
    bool _isChange;
    bool _isFront;
    bool _isOpen;
    DefineUtillHelper.eCardIconType _iconType;

    public DefineUtillHelper.eCardIconType _iType
    {
        get { return _iconType; }
    }

    void Awake()
    {
        _backBG = transform.GetChild(0).gameObject;
        _frontBG = transform.GetChild(1).gameObject;
        _backImage = _backBG.GetComponent<SpriteRenderer>();

        // 임시
        //InitSetCard();
    }


    void Update()
    {
        //if(Input.GetButtonDown("Fire1"))
        //{
        //    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    RaycastHit2D rHit2 = Physics2D.Raycast(pos, transform.forward, Mathf.Infinity);
        //    if(rHit2.collider != null)
        //    {
        //        if(rHit2.collider.gameObject.name.CompareTo(gameObject.name) == 0)
        //        {
        //            ReverseCard(_isOpen = !_isOpen);
        //        }
        //    }
        //}
        if (_isRot)
        {
            if (!_isFront)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * 360);
                if (_isChange && transform.eulerAngles.y <= 270)
                {
                    _isChange = false;
                    ReverseCard(_isOpen = !_isOpen);
                }
                if (transform.eulerAngles.y == 180)
                {
                    _isFront = true;
                    _isRot = false;
                    IngameManger._instance.SelectCard(_no);
                }
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 360);
                if (_isChange && transform.eulerAngles.y <= 270)
                {
                    _isChange = false;
                    ReverseCard(_isOpen = !_isOpen);
                }
                if (transform.eulerAngles.y == 0)
                {
                    _isFront = false;
                    _isRot = false;
                }
            }
        }
    }

    public void InitSetCard(int cardNum, Sprite icon, DefineUtillHelper.eCardIconType type)
    {
        _no = cardNum;
        _iconType = type;
        _icon.sprite = icon;

        _backBG.SetActive(true);
        _frontBG.SetActive(false);
        _isFront = _isOpen = false;
    }

    public void Reverse()
    {
        if(_isFront && !_isRot)
        {
            _isRot = true;
            _isChange = true;
        }
    }

    public void ReverseCard(bool isOpen)
    {
        //Debug.Log(gameObject.name + "을 클릭했습니다.");
        _backBG.SetActive(!isOpen);
        _frontBG.SetActive(isOpen);
    }
    
    void OnMouseDown()
    {
        if (IngameManger._instance._nowFlowState == DefineUtillHelper.eIngameState.CardAction)
        {
            if (!_isFront && !_isRot)
            {
                _isRot = true;
                _isChange = true;
                //ReverseCard(_isOpen = !_isOpen);
            }
        }
    }
}
