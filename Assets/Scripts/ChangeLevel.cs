using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    /*
    ============================================================================
    What Cases Exist For Switching Scenes? When Does SwitchLevel() Get Called?):
    ============================================================================

    ===================
    ButtonFunctions.cs:
    ===================

        - When button "New Game" is clicked, load 'Level1'. This is the same as advancing 
    to the next scene in the build index from the current one (which will be the main menu), 
    assuming that scenes are ordered.

        - When button "Continue" is clicked, load the value stored for PlayerPrefs.Key == 
    'CurrentProgress', if such a key exists. Otherwise, behave the same way as when clicking 
    "New Game" [i.e., advance to the next level in the build index after 'MainMenu' (e.g., 
    'Level1')].
    
    ==========
    Player.cs:
    ==========

        - When Esc is pressed, load 'MainMenu'.
    
    ====================
    TransitionScreen.cs:
    ====================

        - When the condition is 'Lose', restart the current scene (Pass true for restartCurrentLevel)
        
        - When condition is 'Clear', load the next level.
        
        - When condition is 'Win', load 'MainMenu'.

    Advancing to the next scene and going to the main menu both rely on SwitchLevel(), but, 
    rather than passing the argument for targetLevel ourselves (potentially across several 
    different scripts), we can generalize things by creating two new methods called 
    AdvanceToNextLevel() and GoToMainMenu(). 
    */

    public static void SwitchLevel(bool restartCurrentLevel = false, string targetLevel = "")
    {
        if (restartCurrentLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        else
        {
            SceneManager.LoadScene(targetLevel);
        }
    }

    /*
    =====================
    AdvanceToNextLevel():
    =====================

    To advance to the next level, we must first ensure that there IS another level
    whose name we can retrieve for SwitchLevel(). 
    
    endSceneIndex is the count of all Unity scenes minus 1. nextSceneIndex is the 
    current active scene's index plus 1. If nextSceneIndex is less than or equal to 
    endSceneIndex, nextSceneIndex is valid; assign nextLevel with a string representing 
    the target scene's name (extracted from the project's relative path). Otherwise, 
    return an empty string.

    Provided everything is successful (i.e., nextLevel isn't null or empty), call 
    SwitchLevel() with targetLevel as that to which nextLevel refers so that we can 
    switch to the scene in question. If something went wrong and for some reason 
    nextLevel is null or empty (whether due to nextSceneIndex being out-of-bounds 
    or some extraction failure), go to the main menu as a failsafe.
    */

    readonly static int endSceneIndex = SceneManager.sceneCountInBuildSettings - 1;

    public static void AdvanceToNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        string nextLevel = (nextSceneIndex <= endSceneIndex) ? System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(nextSceneIndex)) : "";

        if (!string.IsNullOrEmpty(nextLevel))
        {
            // If nextLevel assignment is successful, PlayerPrefs for current progress
            // need to be set so that the player can continue from this point when
            // clicking "Continue" from the main menu.

            PlayerPrefs.SetString("CurrentProgress", nextLevel);
            PlayerPrefs.Save();
            SwitchLevel(targetLevel: nextLevel);
        }

        else
        {
            GoToMainMenu();
        }
    }

    public static void GoToMainMenu()
    {
        SwitchLevel(targetLevel: "MainMenu");
    }

    /*
    =============================================
    Method for Saving Persistent Game Element(s):
    =============================================

    SaveSettings() is responsible for saving the number of coins that the player 
    has collected between levels. First, it finds an object of type Player, and, 
    once we've verified that one was found, it sets a new PlayerPrefs key entitled 
    "Coins" with the value stored in the object's internal variable for the number 
    of coins that it has collected (retrieved via public property Coins).
    */

    public static void SaveSettings()
    {
        Player playerObject = FindObjectOfType<Player>();

        if (playerObject != null)
        {
            PlayerPrefs.SetInt("Coins", playerObject.Coins);
            PlayerPrefs.Save();
        }
    }
}
