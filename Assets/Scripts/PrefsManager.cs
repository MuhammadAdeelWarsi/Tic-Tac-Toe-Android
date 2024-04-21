using UnityEngine;


public enum ScoreDataHolders
{
    PlayerOneScore,
    PlayerTwoScore
}


public class PrefsManager : MonoBehaviour
{
    /*FIELDS*/
    private static PrefsManager instance;


    /*PROPERTIES*/
    public static PrefsManager Instance => instance;


    /*INITIALIZING METHODS*/
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
        // Debug.Log("Data Holder: " + (int)ScoreDataHolders.PlayerOneScore);
    }


    /*OTHER METHODS*/
    public void SetIntegerData(ScoreDataHolders scoreDataHolder, int newValue)
    {
        PlayerPrefs.SetInt(scoreDataHolder.ToString(), newValue);
    }

    public int GetIntegerData(ScoreDataHolders scoreDataHolder, int defaultValue)
    {
        return PlayerPrefs.GetInt(scoreDataHolder.ToString(), defaultValue);
    }

    public void ResetData()
    {
        Debug.Log("Data Reset!!");
        
        PlayerPrefs.DeleteAll();
    }
}
