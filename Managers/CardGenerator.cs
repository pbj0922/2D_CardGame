using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    GameObject _cardRoot;
    List<CardControl> _cardList;

    float _offsetX = 0.9f;
    float _offsetY = -1.25f;
    Vector3 _genStartPos;
    bool _isEndGenerate = false;

    public CardControl this[int index]
    {
        get { return _cardList[index]; }
    }
    public int _cardCount
    {
        get { return _cardList.Count; }
    }

    public bool _checkEnd
    {
        get { return _isEndGenerate; }
    }

    void Start()
    {
        _cardList = new List<CardControl>();

        _cardRoot = GameObject.FindGameObjectWithTag("CARDROOT");
        _genStartPos = _cardRoot.transform.position;
        GameObject go = GameObject.FindGameObjectWithTag("GameController");

        // 임시
        //if(!GenerateCard(15))
        //{
        //    Debug.Log("카드쌍 개수가 범위를 벗어났습니다.");
        //}
        //StartCoroutine(GenerateCard(15));
        //===
    }

    public void StartGenerate(int pairCount)
    {
        _isEndGenerate = false;
        StartCoroutine(GenerateCard(pairCount));
    }

    public void ListAllClear()
    {
        _cardList.Clear();
    }

    int[] GetArrayFromIconType(int count)
    {
        int[] arr = new int[count];

        int cnt = (int)DefineUtillHelper.eCardIconType.max_IconType;
        int[] temp = new int[cnt];
        // 순서대로 숫자를 입력
        for(int n = 0; n < cnt; n++)
        {
            temp[n] = n;
        }
        // 섞는다
        for(int n = 0; n < 3; n++)
        {
            for(int m = 0; m < cnt; m++)
            {
                int randIdx = Random.Range(0, cnt);
                int num = temp[m];
                temp[m] = temp[randIdx];
                temp[randIdx] = num;
            }
        }
        for (int n = 0; n < arr.Length; n ++)
        {
            arr[n] = temp[n];
        }
        return arr;
    }
    
    IEnumerator GenerateCard(int count)
    {
        if (count < DefineUtillHelper._minCardPairGenerateCount || count > DefineUtillHelper._maxCardPairGenerateCount)
        {
            Debug.Log("카드쌍 개수가 범위를 벗어났습니다.");
        }
        else
        {
            int roopCount = count * 2;
            int[] randIconTypes = GetArrayFromIconType(count);
            int[] iconIndecis = new int[roopCount];

            //test
            //for (int n = 0; n < randIconTypes.Length; n++)
            //{
            //    Debug.Log((n + 1).ToString() + "번쨰 Icon번호 : " + randIconTypes[n]);
            //}
            //===

            for (int n = 0; n < iconIndecis.Length; n++)
            {
                iconIndecis[n] = randIconTypes[n / 2];
            }

            //for (int n = 0; n < iconIndecis.Length; n++)
            //{
            //    Debug.Log((n + 1).ToString() + "번쨰 IconIndecis번호 : " + iconIndecis[n]);
            //}
            for (int n = 0; n < 4; n++)
            {
                for (int m = 0; m < iconIndecis.Length; m++)
                {
                    int randIdx = Random.Range(0, iconIndecis.Length);
                    int num = iconIndecis[m];
                    iconIndecis[m] = iconIndecis[randIdx];
                    iconIndecis[randIdx] = num;
                }
            }

            for (int n = 0; n < roopCount; n++)
            {
                int ix = n % DefineUtillHelper._limitHorizCardCount;
                int iy = n / DefineUtillHelper._limitHorizCardCount;
                Vector3 genPos = _genStartPos + new Vector3(ix * _offsetX, iy * _offsetY);
                GameObject prefab = GameResourcePoolManager._instance.GetPrefabFromKey(DefineUtillHelper.ePrefabType.UI, (int)DefineUtillHelper.ePrefabUIs.CardObj);
                GameObject go = Instantiate(prefab, genPos, Quaternion.identity, _cardRoot.transform);
                CardControl cc = go.GetComponent<CardControl>();
                // 임시
                //DefineUtillHelper.eCardIconType type = (DefineUtillHelper.eCardIconType)Random.Range(0, 30);
                //Sprite img = _mngGame.GetSpriteFromType(type);
                //===
                DefineUtillHelper.eCardIconType type = (DefineUtillHelper.eCardIconType)iconIndecis[n];
                Sprite img = GameResourcePoolManager._instance.GetCardImageFrom(type);
                cc.InitSetCard(n + 1, img, type);
                _cardList.Add(cc);

                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.4f);
        _isEndGenerate = true;
    }
}
