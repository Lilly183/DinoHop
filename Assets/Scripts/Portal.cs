using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    /*
    An object attached with Goal.cs marks the end of the game whenever the player 
    collides with it; whenever the player collides with an object attached with 
    this script (Portal.cs), we want to transition the player to the next level. 
    Perhaps more importantly, we also want to save the number of coins that they've 
    collected using the SaveSettings() method.
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChangeLevel.SaveSettings();
            TransitionScreenController.transitionManagerInstance.transitionScreen.Transition(TransitionScreen.GameState.Clear);
        }
    }
}
