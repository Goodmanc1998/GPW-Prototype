using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class CutSceneTransition : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += MoveToNextScene;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveToNextScene(UnityEngine.Video.VideoPlayer videoPlayer)
    {
        SceneManager.LoadScene(nextScene);

    }
}
