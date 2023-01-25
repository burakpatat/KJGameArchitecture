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
    public TMP_Text CoinCountText;
    public TMP_Text GemCountText;

    void Update()
    {
        GetStar(StarCount_e.TwoStar);
    }
    public void GetStar(Enum _enum)
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
                break;
            case StarCount_e.TwoStar:
                StarText.text = "BRAVO!";
                break;
            case StarCount_e.ThreeStar:
                StarText.text = "HARÝKASIN!";
                break;

        }
    }
}
