using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /*FIELDS*/
    private static UIManager instance;
    private static bool isFirstTimeLoaded;
    private bool isGameStarting;

    [SerializeField] GameObject gameBackground;
    [SerializeField] GameObject header;
    [SerializeField] GameObject footer;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject gameBoardPanel;
    [SerializeField] GameObject screenBlockPanel;
    [SerializeField] GameObject gameTiedPanel;
    [SerializeField] GameObject finalPanel;
    [SerializeField] GameObject[] playerIndicators;
    [SerializeField] Text[] scoreTexts;
    [SerializeField] Button proceedButton;


    /*PROPERTIES*/
    public static UIManager Instance => instance;


    /*INITIALIZING METHODS*/
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
    }

    private void Start() 
    {
        if(!isFirstTimeLoaded)
        {
            EnableLoadingPanel();
        }
        else
        {
            EnableGameBackground();
        }

        isGameStarting = true;
    }


    /*OTHER METHODS*/
    private void EnableLoadingPanel()
    {
        isFirstTimeLoaded = true;

        loadingPanel.SetActive(true);
        StartCoroutine(AudioManager.Instance.PlayBackgroundMusic(2, 7, 8f, true));
        StartCoroutine(AudioManager.Instance.StopSoundOrMusic(2, 8.75f));
        Invoke("EnableGameBackground", 9.5f);
    }
    
    private void EnableFinalPanel()
    {
        finalPanel.SetActive(true);
        Invoke("DisableGameBoardContents", 0.6f);
    }
    
    private void EnableGameBackground()
    {
        gameBackground.SetActive(true);
        Invoke("EnableGameBoardContents", 1.5f);
    }

    private void EnableProceedButton()
    {
        proceedButton.gameObject.SetActive(true);
    }

    private void EnableGameBoardContents()
    {
        loadingPanel.SetActive(false);

        header.SetActive(true);
        footer.SetActive(true);
        gameBoardPanel.SetActive(true);

        EnablePlayersIndicator();
    }

    private void DisableScreenBlockPanel()
    {
        screenBlockPanel.SetActive(false);
    }

    private void DisableGameBoardContents()     //Done to stop all unnecessary animations behind the Final Panel
    {
        gameBackground.SetActive(false);
        header.SetActive(false);
        footer.SetActive(false);
        gameBoardPanel.SetActive(false);
        if(gameTiedPanel.activeSelf)
        {
            gameTiedPanel.SetActive(false);
        }
    }

    private void EnablePlayersIndicator()
    {
        foreach(GameObject playerIndicator in playerIndicators)
        {
            playerIndicator.SetActive(true);
        }
    }

    private void EnablePlayersScore()
    {
        UpdateAllPlayersScoreTexts();

        foreach(Text scoreText in scoreTexts)
        {
            scoreText.gameObject.SetActive(true);
        }
    }

    private void UpdateAllPlayersScoreTexts()
    {
        Debug.Log("Updating Score Texts!!");

        foreach(ScoreDataHolders scoreDataHolder in Enum.GetValues(typeof(ScoreDataHolders)))
        {
            int scoreDataHolderId = (int)scoreDataHolder;
            scoreTexts[scoreDataHolderId].text = PrefsManager.Instance.GetIntegerData(scoreDataHolder, 0).ToString();
        }
    }

    public void DisableProceedButton()
    {
        AudioManager.Instance.PlayUserInterfaceSound(0);

        proceedButton.interactable = false;
        proceedButton.GetComponent<Animator>().SetTrigger("DisableTrigger");

        Invoke("EnableFinalPanel", 1f);
    }

    public void UpdatePlayersScoreData(int scoreDataHolderId)
    {
        Debug.Log("Updating Score Data!!");
        
        ScoreDataHolders winnerScoreDataHolder = Enum.GetValues(typeof(ScoreDataHolders)).Cast<ScoreDataHolders>().Single(scoreDataHolder => (int)scoreDataHolder == scoreDataHolderId);

        int winnerNewScore = PrefsManager.Instance.GetIntegerData(winnerScoreDataHolder, 0) + 1;
        PrefsManager.Instance.SetIntegerData(winnerScoreDataHolder, winnerNewScore);

        StartCoroutine(UpdatePlayerScoreText(scoreDataHolderId, winnerNewScore));
    }

    public void AnimatePlayerIndicator(int currentTurnId, bool hasTied, bool hasWon)
    {
        if(hasTied)
        {
            playerIndicators[currentTurnId].GetComponent<Animator>().SetTrigger("InactiveTrigger");
            gameTiedPanel.SetActive(true);
            Invoke("EnableProceedButton", 2.75f);
        }
        else if(hasWon)
        {
            playerIndicators[currentTurnId].GetComponent<Animator>().SetTrigger("WinTrigger");
            Invoke("EnableProceedButton", 4.75f);
        }
        else
        {
            playerIndicators[currentTurnId].GetComponent<Animator>().SetTrigger("ActiveTrigger");
            playerIndicators.Single(element => Array.IndexOf(playerIndicators, element) != currentTurnId).GetComponent<Animator>().SetTrigger("InactiveTrigger");

            if(isGameStarting)
            {
                isGameStarting = false;
                EnablePlayersScore();
                Invoke("DisableScreenBlockPanel", 2f);
            }
        }
    }

    public void ChangeGame(bool reset)
    {
        AudioManager.Instance.PlayUserInterfaceSound(0);
        
        if(reset)
        {
            PrefsManager.Instance.ResetData();
        }

        finalPanel.GetComponent<Animator>().SetTrigger("DisableTrigger");

        StartCoroutine(GameBoard.Instance.RestartGame(1.75f));
    }


    /*COROUTINES*/
    private IEnumerator UpdatePlayerScoreText(int scoreDataHolderId, int newScore)
    {
        scoreTexts[scoreDataHolderId].GetComponent<Animator>().SetTrigger("WinScaleTrigger");

        yield return new WaitForSeconds(0.85f);

        scoreTexts[scoreDataHolderId].text = newScore.ToString();
    }
}
