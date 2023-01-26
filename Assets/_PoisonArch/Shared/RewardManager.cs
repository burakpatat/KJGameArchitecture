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
    int StarCount; int GemCount;

    const string k_StarCount = "StarCount";
    public int S_StarCount
    {
        get => PlayerPrefs.GetInt(k_StarCount);
        set => PlayerPrefs.SetInt(k_StarCount, value);
    }
    void Update()
    {
        GetStar(StarCount_e.TwoStar, 0);
    }
    public void GetStar(Enum _enum, int _StarCount)
    {
        StarCountEnum = (StarCount_e)_enum;
        int _openCount = (int)Enum.ToObject(StarCountEnum.GetType(), StarCountEnum);

        for (int i = 0; i < _openCount; i++)
        {
            Stars[i].transform.GetChild(1).gameObject.SetActive(true);
        }

        switch (StarCountEnum)
        {
            case StarCount_e.OneStar:
                StarText.text = "TEBRÝKLER!";
                S_StarCount += 1;
                break;
            case StarCount_e.TwoStar:
                StarText.text = "BRAVO!";
                S_StarCount += 2;
                break;
            case StarCount_e.ThreeStar:
                StarText.text = "HARÝKASIN!";
                S_StarCount += 3;
                break;

        }

        SaveStarData(S_StarCount);
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
