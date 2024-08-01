using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    // We can directly specify the argument for an arbitrary parameter in a method call using
    // syntax paramName: paramValue. This is the concept of named arguments; we're matching via
    // name rather than position, which frees us from having to match the order of the arguments
    // to the order by which parameters appear in the parameter list of the invoked method.
    
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("CurrentProgress"))
        {
            ChangeLevel.SwitchLevel(targetLevel: PlayerPrefs.GetString("CurrentProgress"));
        }

        else
        {
            ChangeLevel.AdvanceToNextLevel();
        }
    }

    // Remember that the player's access to these buttons means that the current scene is
    // MainMenu. The player is either starting from the Main Menu or has returned to it,
    // which means NewGame() should always advance to the next scene (A.K.A., "Level1").

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        ChangeLevel.AdvanceToNextLevel();
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}