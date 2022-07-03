using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataManager
{
    private const string _recordDataName = "record";


    // Record Data functions

    public int GetRecord()
    {
        return PlayerPrefs.GetInt(_recordDataName);
    }

    public bool TrySetRecord(int points)
    {
        int record = GetRecord();

        if(points > record)
        {
            PlayerPrefs.SetInt(_recordDataName, points);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetRecord()
    {

        PlayerPrefs.SetInt(_recordDataName, 0);
    }
}
