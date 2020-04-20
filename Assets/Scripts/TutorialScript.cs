using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    //Bools used to store if each spell has been casted 
    public bool spellOneCast;
    public bool spellTwoCast;
    public bool spellThreeCast;

    //An overall bool to confirm once tutorial is complete
    public bool tutorialComplete;

    //Door animation
    public Animator doorAnimation;

    //A count used to make sure the 
    int count;

    // Update is called once per frame
    void Update()
    {
        //Checking if all spells have been casted and that the tuorial is not complete
        if(spellOneCast && spellTwoCast && spellThreeCast && tutorialComplete != true)
        {
            //Setting the tutorial to complete
            tutorialComplete = true;
        }

        //Checking if the tutorial is complete
        if (tutorialComplete && count < 1)
        {
            //Playing the door animation 
            doorAnimation.SetBool("Dooropen", true);
            count++;
        }
    }

}
