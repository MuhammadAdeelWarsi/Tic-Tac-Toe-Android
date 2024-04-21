using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameBoard : MonoBehaviour
{
    /*FIELDS*/
    private static GameBoard instance;
    private int currentTurnId;
    private int turnsPlayed;
    private bool playerHasWon;
    private PlayerInput[] successfulPlayerInputs;

    [SerializeField] float buttonsSpawnDelay;
    [SerializeField] PlayerInput[] playerInputs;


    /*PROPERTIES*/
    public static GameBoard Instance => instance;
    public int CurrentTurnId => currentTurnId;
    public bool PlayerHasWon => playerHasWon;
    public int TurnsPlayed
    {
        get{return turnsPlayed;}
        set{turnsPlayed = value;}
    }


    /*INITIALIZING METHODS*/
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
        
        successfulPlayerInputs = new PlayerInput[3];
        
        currentTurnId = UnityEngine.Random.Range(0, 2);
        Debug.Log("First Turn Id: " + currentTurnId);
    }

    private void Start() 
    {
        StartCoroutine(SpawnInputButtons());    
    }


    /*OTHER METHODS*/
    private void GameCompleted(bool hasTied)
    {
        foreach(PlayerInput playerInput in playerInputs)
        {
            playerInput.InputButton.interactable = false;
            playerInput.GetComponent<EventTrigger>().enabled = false;
        }

        if(hasTied)
        {
            Debug.Log("Game Tied!!");
            StartCoroutine(AudioManager.Instance.PlayBackgroundMusic(1, 4, 0.1f));

            UIManager.Instance.AnimatePlayerIndicator(currentTurnId, hasTied, false);
        }
        else
        {
            Debug.Log("Game Won!!");
            StartCoroutine(AudioManager.Instance.PlayBackgroundMusic(1, 5, 0.5f));

            foreach(PlayerInput successfulPlayerInput in successfulPlayerInputs)
            {
                successfulPlayerInput.Animator.SetTrigger("GameWin");
            }

            UIManager.Instance.AnimatePlayerIndicator(currentTurnId, hasTied, true);

            UIManager.Instance.UpdatePlayersScoreData(currentTurnId);
        }
    }

    private void SeparateSuccessfulPlayerInputs(PlayerInput[] successfulPlayerInputs)
    {
        for(int i = 0; i < this.successfulPlayerInputs.Length; i++)
        {
            this.successfulPlayerInputs[i] = successfulPlayerInputs[i];
        }
    }

    public void UpdateCurrentTurnId(int currentTurnId)
    {
        this.currentTurnId = currentTurnId;
        UIManager.Instance.AnimatePlayerIndicator(this.currentTurnId, false, false);
    }

    public bool CheckGameCompleted(string currentTurnHolderText)
    {
        Debug.Log("Checking Game Result!!");

        if(playerInputs[0].InputText.text == currentTurnHolderText && playerInputs[1].InputText.text == currentTurnHolderText && playerInputs[2].InputText.text == currentTurnHolderText)
        {
            SeparateSuccessfulPlayerInputs(new PlayerInput[]{playerInputs[0], playerInputs[1], playerInputs[2]});
            GameCompleted(false);
            return true;
        }

        else if(playerInputs[3].InputText.text == currentTurnHolderText && playerInputs[4].InputText.text == currentTurnHolderText && playerInputs[5].InputText.text == currentTurnHolderText)
        {
            SeparateSuccessfulPlayerInputs(new PlayerInput[]{playerInputs[3], playerInputs[4], playerInputs[5]});
            GameCompleted(false);
            return true;
        }

        else if(playerInputs[6].InputText.text == currentTurnHolderText && playerInputs[7].InputText.text == currentTurnHolderText && playerInputs[8].InputText.text == currentTurnHolderText)
        {
            SeparateSuccessfulPlayerInputs(new PlayerInput[]{playerInputs[6], playerInputs[7], playerInputs[8]});
            GameCompleted(false);
            return true;
        }

        else if(playerInputs[0].InputText.text == currentTurnHolderText && playerInputs[3].InputText.text == currentTurnHolderText && playerInputs[6].InputText.text == currentTurnHolderText)
        {
            SeparateSuccessfulPlayerInputs(new PlayerInput[]{playerInputs[0], playerInputs[3], playerInputs[6]});
            GameCompleted(false);
            return true;
        }

        else if(playerInputs[1].InputText.text == currentTurnHolderText && playerInputs[4].InputText.text == currentTurnHolderText && playerInputs[7].InputText.text == currentTurnHolderText)
        {
            SeparateSuccessfulPlayerInputs(new PlayerInput[]{playerInputs[1], playerInputs[4], playerInputs[7]});
            GameCompleted(false);
            return true;
        }

        else if(playerInputs[2].InputText.text == currentTurnHolderText && playerInputs[5].InputText.text == currentTurnHolderText && playerInputs[8].InputText.text == currentTurnHolderText)
        {
            SeparateSuccessfulPlayerInputs(new PlayerInput[]{playerInputs[2], playerInputs[5], playerInputs[8]});
            GameCompleted(false);
            return true;
        }

        else if(playerInputs[0].InputText.text == currentTurnHolderText && playerInputs[4].InputText.text == currentTurnHolderText && playerInputs[8].InputText.text == currentTurnHolderText)
        {
            SeparateSuccessfulPlayerInputs(new PlayerInput[]{playerInputs[0], playerInputs[4], playerInputs[8]});
            GameCompleted(false);
            return true;
        }

        else if(playerInputs[2].InputText.text == currentTurnHolderText && playerInputs[4].InputText.text == currentTurnHolderText && playerInputs[6].InputText.text == currentTurnHolderText)
        {
            SeparateSuccessfulPlayerInputs(new PlayerInput[]{playerInputs[2], playerInputs[4], playerInputs[6]});
            GameCompleted(false);
            return true;
        }

        else if(turnsPlayed >= 9)
        {
            GameCompleted(true);
            return true;
        }

        return false;
    }


    /*COROUTINES*/
    private IEnumerator SpawnInputButtons()
    {
        StartCoroutine(AudioManager.Instance.PlayBackgroundMusic(1, 2, 0f, true));

        foreach(PlayerInput playerInput in playerInputs)
        {
            playerInput.gameObject.SetActive(true);
            yield return new WaitForSeconds(buttonsSpawnDelay);
        }

        AudioManager.Instance.StopSoundOrMusic(1);

        UIManager.Instance.AnimatePlayerIndicator(currentTurnId, false, false);
    }

    public IEnumerator RestartGame(float secondsDelay)
    {
        Debug.Log("Restarting New Game!!");
        
        yield return new WaitForSeconds(secondsDelay);
        SceneManager.LoadScene(0);
    }
}
