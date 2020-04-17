using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public bool spellOneCast;
    public bool spellTwoCast;
    public bool spellThreeCast;

    public bool tutorialComplete;

    public Animator doorAnimation;

    int count;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(spellOneCast && spellTwoCast && spellThreeCast && tutorialComplete != true)
        {
            tutorialComplete = true;
        }

        if (tutorialComplete && count < 1)
        {
            doorAnimation.SetBool("Dooropen", true);
            count++;
        }
    }

}
