using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuCanvasManager : CanvasManagerBase
{
    public static MenuCanvasManager Instance { get; private set; }

    public TMP_Text RecordValue_Text;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void Start()
    {
        UpdateRecordValueText();
    }

    public void OnClickStart()
    {
        CoreManager.Instance.StartGameplay();
    }
    public void OnClickResetRecord()
    {
        CoreManager.Instance.PlayerData.ResetRecord();
        UpdateRecordValueText();
    }

    private void UpdateRecordValueText()
    {
        RecordValue_Text.text = "" + CoreManager.Instance.PlayerData.GetRecord();
    }
}
