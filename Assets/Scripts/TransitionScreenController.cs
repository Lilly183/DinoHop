using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScreenController : MonoBehaviour
{
    /*
    ===================
    Transition Manager:
    ===================

    TransitionScreenController uses a singleton pattern. One member is an object 
    of the class itself and is declared static; it exists without the need to 
    instantiate an object of the class. Because it's also public, we're making it 
    accessible to the outside world. This ensures that the TransitionScreenController 
    class has just one instance, and that this single instance is accessible from 
    other scripts.

    Through this object, we're capable of accessing another member variable called 
    transitionScreen, which — as the name suggests — is a reference to an object 
    of class TransitionScreen.

    In this way — via transitionManagerInstance and then via a reference to an 
    object of class transitionScreen — we can set whether or not the latter is 
    active (enabled/disabled), as well as have access to the Transition() method 
    from the TransitionScreen class, which uses a switch statement to decide between 
    invoking LoadCurrent(), LoadNext(), or LoadMenu().

    ==============================================================================

    The approach used to display a popup screen to the player in this manner dates 
    back to code that was first used for Assignment 2, then Assignment 3. Though it
    has been reworked extensively, credit for the original idea still belongs to Coco 
    Code!
    https://www.youtube.com/watch?v=K4uOjb5p3Io
    */

    //===================
    // Singleton Pattern:
    //===================

    public static TransitionScreenController transitionManagerInstance;

    //=======================================
    // Makes Reference(s) to Other Object(s):
    //=======================================

    public TransitionScreen transitionScreen;

    private void Start()
    {
        if (transitionManagerInstance == null)
        {
            transitionManagerInstance = this;
        }
    }
}
