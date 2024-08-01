using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    /*
    When the trigger fires for an object with this script, check the tag 
    of the colliding object. If tagged as "Player", the player has reached 
    the goal and won the game. Call Transition() and pass the condition 'Win' 
    to bring up the "You Win!" screen.
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TransitionScreenController.transitionManagerInstance.transitionScreen.Transition(TransitionScreen.GameState.Win);
        }
    }
}
