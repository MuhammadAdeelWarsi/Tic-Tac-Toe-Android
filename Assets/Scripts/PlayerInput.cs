using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    /*FIELDS*/
    private bool isFinalInput;
    private string currentTurnHolderText;

    [SerializeField] Button inputButton;
    [SerializeField] Text inputText;
    [SerializeField] Animator animator;


    /*PROPERTIES*/
    public Button InputButton => inputButton;
    public Text InputText => inputText;
    public Animator Animator => animator;


    /*OTHER METHODS*/
    public void TakeInput()
    {
        gameObject.GetComponent<EventTrigger>().enabled = false;
        AudioManager.Instance.PlayUserInterfaceSound(3);

        int currentTurnId = GameBoard.Instance.CurrentTurnId;
        currentTurnHolderText = currentTurnId == 0 ? "X" : "O";

        Debug.Log("Input Taken!!");

        inputButton.interactable = false;
        inputText.text = currentTurnHolderText;

        GameBoard.Instance.TurnsPlayed++;

        if(GameBoard.Instance.TurnsPlayed > 4)
        {
            isFinalInput = GameBoard.Instance.CheckGameCompleted(currentTurnHolderText);
        }

        if(isFinalInput)
        {
            return;
        }
        else
        {
            if(currentTurnId == 0)
            {
                GameBoard.Instance.UpdateCurrentTurnId(1);
            }
            else
            {
                GameBoard.Instance.UpdateCurrentTurnId(0);
            }
        }
    }
}
