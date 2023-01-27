using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoisonArch;

using TMPro;
using DG.Tweening;
using System;

public enum StarCount_e
{
    OneStar = 1,
    TwoStar,
    ThreeStar
}
public class RewardManager : AbstractSingleton<AudioManager>
{
    [Header("Star")]
    public List<Transform> Stars;
    private StarCount_e StarCountEnum;
    public TMP_Text StarText;

    [Header("Coin & Gem")]
    public TMP_Text StarCountText;
    public TMP_Text GemCountText;
    int GemCount;

    const string k_StarCount = "StarCount";
    public int S_StarCount
    {
        get => PlayerPrefs.GetInt(k_StarCount);
        set => PlayerPrefs.SetInt(k_StarCount, value);
    }
    private void Awake()
    {
        StarCountText.text = LoadStarData().ToString();
    }
    void Update()
    {
        GetStar(StarCount_e.TwoStar);
    }
    public void GetStar(Enum _enum)
    {
        StartCoroutine(StarCoroutine(_enum));

        Sequence DCSeq = DOTween.Sequence().SetAutoKill(false);

        switch (StarCountEnum)
        {
            case StarCount_e.OneStar:
                StarText.text = "TEBR�KLER!";
                S_StarCount += 1;
                StarCountText.text = S_StarCount.ToString();

                DCSeq.Append(Stars[0].GetComponent<RectTransform>().DOScale(new Vector3(1.1f, 1.1f, 1.1f), .4f));
                DCSeq.Append(Stars[0].GetComponent<RectTransform>().DOScale(Vector3.one, .2f));
                DCSeq.Append(StarText.GetComponent<RectTransform>().DOScale(Vector3.one, .5f));
                break;
            case StarCount_e.TwoStar:
                StarText.text = "BRAVO!";
                S_StarCount += 2;
                StarCountText.text = S_StarCount.ToString();

                DCSeq.Append(Stars[0].GetComponent<RectTransform>().DOScale(new Vector3(1.2f, 1.2f, 1.2f), .4f));
                DCSeq.Join(Stars[0].GetComponent<RectTransform>().DOScale(new Vector3(1f, 1f, 1f), .4f));
                DCSeq.Append(Stars[1].GetComponent<RectTransform>().DOScale(new Vector3(1.25f, 1.25f, 1.25f), .4f));
                DCSeq.Append(Stars[1].GetComponent<RectTransform>().DOScale(new Vector3(1.2f, 1.2f, 1.2f), .4f));
                DCSeq.Join(StarText.GetComponent<RectTransform>().DOScale(Vector3.one, .5f));
                break;
            case StarCount_e.ThreeStar:
                StarText.text = "HAR�KASIN!";
                S_StarCount += 3;
                StarCountText.text = S_StarCount.ToString();

                DCSeq.Append(Stars[0].GetComponent<RectTransform>().DOScale(new Vector3(1.2f, 1.2f, 1.2f), .4f));
                DCSeq.Join(Stars[0].GetComponent<RectTransform>().DOScale(Vector3.one, .4f));
                DCSeq.Append(Stars[1].GetComponent<RectTransform>().DOScale(new Vector3(1.25f, 1.25f, 1.25f), .4f));
                DCSeq.Join(Stars[1].GetComponent<RectTransform>().DOScale(new Vector3(1.2f, 1.2f, 1.2f), .4f));
                DCSeq.Append(Stars[2].GetComponent<RectTransform>().DOScale(new Vector3(1.1f, 1.1f, 1.1f), .4f));
                DCSeq.Append(Stars[2].GetComponent<RectTransform>().DOScale(Vector3.one, .2f));
                DCSeq.Append(StarText.GetComponent<RectTransform>().DOScale(Vector3.one, .5f));
                break;
        }

        SaveStarData(S_StarCount);
    }
    IEnumerator StarCoroutine(Enum _enum)
    {
        StarCountEnum = (StarCount_e)_enum;
        int _openCount = (int)Enum.ToObject(StarCountEnum.GetType(), StarCountEnum);

        for (int i = 0; i < _openCount; i++)
        {
            Stars[i].transform.GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(.4f);
        }
    }

    public int LoadStarData()
    {
        return PlayerPrefsUtils.Read<int>(k_StarCount);
    }
    public void SaveStarData(int _starCount)
    {
        PlayerPrefsUtils.Write(k_StarCount, _starCount);
    }
}
