using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayCanvasManager : MonoBehaviour
{
    public static GameplayCanvasManager Instance { get; private set; }

    public TMP_Text ScoreValue_Text;
    public RawImage[] Hearts_Images;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UpdateScoreValueText(0);
    }

    public void UpdateScoreValueText(int value)
    {
        ScoreValue_Text.text = "" + value;
    }

    public void UpdateHealthValue(int hp)
    {
        int difference = 3 - hp;
        for(int i = 0; i < difference; i++)
        {
            Hearts_Images[i].gameObject.SetActive(false);
        }
    }
}
