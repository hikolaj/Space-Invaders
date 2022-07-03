using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenCanvasManager : MonoBehaviour
{
    public static EndScreenCanvasManager Instance { get; private set; }

    public TMP_Text ScoreValue_Text;
    public TMP_Text RecordValue_Text;
    public TMP_Text NewRecord_Text;

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
        int points = GameplayManager.Instance.Points;
        int oldRecord = CoreManager.Instance.PlayerData.GetRecord();
        bool isNewRecord = CoreManager.Instance.PlayerData.TrySetRecord(points);

        ScoreValue_Text.text = "" + points;
        RecordValue_Text.text = "" + oldRecord;
        NewRecord_Text.gameObject.SetActive(isNewRecord);

    }

    public void OnClickMenu()
    {
        CoreManager.Instance.OpenMenu();
    }
}
