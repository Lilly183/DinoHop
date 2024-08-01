using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionScreen : MonoBehaviour
{
    //=======================================
    // Makes Reference(s) to Other Object(s):
    //=======================================

    public Text transitionScreenText;

    //====================
    // Member Variable(s):
    //====================
    
    public float transitionDelay = 2.5f;
    public enum GameState : ushort { Lose = 0, Clear = 1, Win = 2};

    public void Transition(GameState condition)
    {
        gameObject.SetActive(true);

        switch (condition)
        {
            case GameState.Lose:
                transitionScreenText.text = "<color=#FF0000>You Died</color>";
                Invoke(nameof(LoadCurrent), transitionDelay);
                break;
            case GameState.Clear:
                transitionScreenText.text = "Stage Cleared!";
                Invoke(nameof(LoadNext), transitionDelay);
                break;
            case GameState.Win:
                transitionScreenText.text = "<color=#06FF01>You Win!</color>";
                Invoke(nameof(LoadMenu), transitionDelay);
                break;
            default:
                break;
        }
    }

    private void LoadCurrent()
    {
        ChangeLevel.SwitchLevel(restartCurrentLevel: true);
    }

    private void LoadNext()
    {
        ChangeLevel.AdvanceToNextLevel();
    }

    private void LoadMenu()
    {
        ChangeLevel.GoToMainMenu();
    }
}
